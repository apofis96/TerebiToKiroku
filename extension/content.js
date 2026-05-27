// content.js — injected on youtube.com/watch pages

(function () {
  let sessionStart = null;
  let sessionWatchedSeconds = 0;
  let lastCurrentTime = null;
  let tickInterval = null;
  let video = null;
  let videoId = null;

  /* ── helpers ── */
  function getVideoId() {
    const params = new URLSearchParams(window.location.search);
    return params.get("v");
  }

  function getTitle() {
    return document.title.replace(" - YouTube", "").trim();
  }

  function getDuration() {
    return video ? Math.round(video.duration) || 0 : 0;
  }

  function getCurrentTime() {
    return video ? Math.round(video.currentTime) || 0 : 0;
  }

  function isPlaying() {
    return video && !video.paused && !video.ended;
  }

  /* ── storage helpers ── */
  async function loadRecord(id) {
    return new Promise((res) => {
      chrome.storage.local.get(["ytTracker_" + id], (r) => {
        res(r["ytTracker_" + id] || null);
      });
    });
  }

  async function saveRecord(id, data) {
    return new Promise((res) => {
      chrome.storage.local.set({ ["ytTracker_" + id]: data }, res);
    });
  }

  async function loadHistory() {
    return new Promise((res) => {
      chrome.storage.local.get(["ytTracker_history"], (r) => {
        res(r["ytTracker_history"] || []);
      });
    });
  }

  async function pushHistory(id) {
    const history = await loadHistory();
    if (!history.includes(id)) {
      history.unshift(id);
      if (history.length > 50) history.pop();
      await new Promise((res) =>
        chrome.storage.local.set({ ytTracker_history: history }, res)
      );
    }
  }

  /* ── session tracking ── */
  async function tick() {
    if (!video || !videoId) return;
    if (!isPlaying()) return;

    const ct = getCurrentTime();
    // only count if time advanced naturally (≤2s gap)
    if (lastCurrentTime !== null && ct > lastCurrentTime && ct - lastCurrentTime <= 2) {
      sessionWatchedSeconds += ct - lastCurrentTime;
    }
    lastCurrentTime = ct;

    const record = (await loadRecord(videoId)) || {
      id: videoId,
      title: getTitle(),
      duration: getDuration(),
      watchedSeconds: 0,
      lastPosition: 0,
      firstSeen: Date.now(),
      lastSeen: Date.now(),
    };

    record.title = getTitle();
    record.duration = getDuration();
    record.lastPosition = ct;
    record.watchedSeconds = (record.watchedSeconds || 0) + 1; // increment by real-tick seconds
    record.lastSeen = Date.now();

    await saveRecord(videoId, record);
    await pushHistory(videoId);

    // Broadcast to popup if open
    chrome.runtime.sendMessage({
      type: "YT_TICK",
      data: {
        id: videoId,
        title: record.title,
        duration: record.duration,
        currentTime: ct,
        watchedSeconds: record.watchedSeconds,
        progress: record.duration ? ct / record.duration : 0,
        playbackRate: video.playbackRate,
      },
    }).catch(() => {});
  }

  /* ── init ── */
  function attachVideo(v) {
    video = v;
    videoId = getVideoId();
    sessionStart = Date.now();
    sessionWatchedSeconds = 0;
    lastCurrentTime = null;

    if (tickInterval) clearInterval(tickInterval);
    tickInterval = setInterval(tick, 1000);
  }

  function findAndAttach() {
    const v = document.querySelector("video");
    if (v && v !== video) {
      attachVideo(v);
    }
  }

  // YouTube is a SPA — watch for navigation
  const observer = new MutationObserver(() => {
    const newId = getVideoId();
    if (newId && newId !== videoId) {
      findAndAttach();
    }
  });
  observer.observe(document.body, { childList: true, subtree: true });

  findAndAttach();

  // Handle popup asking for current state
  chrome.runtime.onMessage.addListener((msg, _sender, sendResponse) => {
    if (msg.type === "GET_CURRENT") {
      if (!video || !videoId) {
        sendResponse(null);
        return;
      }
      sendResponse({
        id: videoId,
        title: getTitle(),
        duration: getDuration(),
        currentTime: getCurrentTime(),
        progress: getDuration() ? getCurrentTime() / getDuration() : 0,
        playing: isPlaying(),
        playbackRate: video.playbackRate,
      });
    }
  });
})();
