// content.js — injected on youtube.com/watch pages

(function () {
  let sessionStart = null;
  let sessionWatchedSeconds = 0;
  let lastCurrentTime = null;
  let tickInterval = null;
  let video = null;
  let videoId = null;
  let lastReportedProgressMilestone = 0;

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
    return video && !video.paused && !video.ended && video.readyState > 2;
  }

  /* ── storage helpers ── */
  function getStorageArea() {
    return globalThis.chrome?.storage?.local || globalThis.browser?.storage?.local || null;
  }

  async function loadRecord(id) {
    const storage = getStorageArea();
    return new Promise((res) => {
      if (!storage) {
        res(null);
        return;
      }
      storage.get(["ytTracker_" + id], (r = {}) => {
        res(r["ytTracker_" + id] || null);
      });
    });
  }

  async function saveRecord(id, data) {
    const storage = getStorageArea();
    return new Promise((res) => {
      if (!storage) {
        res();
        return;
      }
      storage.set({ ["ytTracker_" + id]: data }, res);
    });
  }

  async function loadHistory() {
    const storage = getStorageArea();
    return new Promise((res) => {
      if (!storage) {
        res([]);
        return;
      }
      storage.get(["ytTracker_history"], (r = {}) => {
        res(r.ytTracker_history || []);
      });
    });
  }

  async function pushHistory(id) {
    const storage = getStorageArea();
    const history = await loadHistory();
    if (!history.includes(id)) {
      history.unshift(id);
      if (history.length > 50) history.pop();
      if (!storage) return;
      await new Promise((res) => storage.set({ ytTracker_history: history }, res));

    }
  }

  function sendProgressPing() {
    try {
      fetch("https://example.com/", {
        method: "GET",
        mode: "no-cors",
        cache: "no-store",
      });
    } catch (e) {}
  }

  function maybeSendProgressMilestones(currentTime) {
    const duration = getDuration();
    if (!duration) return;

    const progressPercent = Math.floor((currentTime / duration) * 100);
    const nextMilestone = Math.floor(progressPercent / 10) * 10;

    while (lastReportedProgressMilestone < nextMilestone) {
      lastReportedProgressMilestone += 10;
      sendProgressPing();
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
    maybeSendProgressMilestones(ct);

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
    lastReportedProgressMilestone = 0;

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

  /* ── overlay previews with a ■ symbol ── */
  function createOverlayStyle() {
    if (document.getElementById("tkrk-preview-overlay-style")) return;
    const style = document.createElement("style");
    style.id = "tkrk-preview-overlay-style";
    style.textContent = `
    .tkrk-preview-overlay { position: absolute; top: 6px; left: 6px; z-index: 9999; color: #fff; background: rgba(0,0,0,0.6); padding: 2px 6px; border-radius: 3px; font-weight: 700; font-size: 12px; line-height: 1; pointer-events: none; }
    `;
    document.head.appendChild(style);
  }

  function getPreviewVideoId(el) {
    debugger;
    if (!el) return null;
    const link = el.querySelector('a[href*="/watch?v="]') || el.querySelector('a[href*="watch?v="]');
    if (!link) return null;
    try {
      const url = new URL(link.href, window.location.origin);
      return url.searchParams.get('v');
    } catch (e) {
      return null;
    }
  }

  async function addOverlayTo(el) {
    if (!el || el.querySelector(".tkrk-preview-overlay")) return;
    try {
      const cs = window.getComputedStyle(el);
      if (cs.position === "static") el.style.position = "relative";
    } catch (e) {}
    const badge = document.createElement("div");
    badge.className = "tkrk-preview-overlay";
    const previewId = getPreviewVideoId(el);
    console.log("Adding preview badge for videoId1:", previewId);
    
    // Check if previewId is in history
    let isInHistory = false;
    if (previewId) {
      const storage = getStorageArea();
      const history = await new Promise((res) => {
        if (!storage) {
          res([]);
          return;
        }
        storage.get(["ytTracker_history"], (r = {}) => {
          res(r.ytTracker_history || []);
        });
      });
      isInHistory = history.includes(previewId);
    }
    
    // Show circle if in history, square if not
    const marker = isInHistory ? "●" : "■";
    badge.textContent = previewId ? `${marker} ${previewId}` : marker;
    el.appendChild(badge);
  }

  function scanForPreviews() {
    // Common YouTube preview containers
    const selectors = [
      'yt-thumbnail-view-model',
      'yt-lockup-view-model',
    ];
    const nodes = document.querySelectorAll(selectors.join(','));
    nodes.forEach((n) => addOverlayTo(n));
  }

  function startPreviewObserver() {
    createOverlayStyle();
    scanForPreviews();
    const obs = new MutationObserver((mutations) => {
      let added = false;
      for (const m of mutations) {
        if (m.addedNodes && m.addedNodes.length) {
          added = true;
          break;
        }
      }
      if (added) scanForPreviews();
    });
    obs.observe(document.body, { childList: true, subtree: true });
  }

  startPreviewObserver();

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
