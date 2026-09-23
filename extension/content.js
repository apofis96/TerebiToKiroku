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

  async function loadProgressMilestone(id) {
    const storage = getStorageArea();
    return new Promise((res) => {
      if (!storage) {
        res(0);
        return;
      }
      storage.get(["ytTracker_progress_" + id], (r = {}) => {
        const value = r["ytTracker_progress_" + id];
        res(typeof value === "number" ? value : 0);
      });
    });
  }

  async function getMarkerForId(id) {
    const history = await loadHistory();
    const record = await loadRecord(id);
    const duration = Number(record?.duration || 0);
    const watchedSeconds = Number(record?.watchedSeconds || 0);
    const isFullyWatched = Boolean(record && duration > 0 && watchedSeconds >= duration);
    const isInHistory = history.includes(id);
    return isFullyWatched ? '▲' : isInHistory ? '●' : '■';
  }

  async function saveProgressMilestone(id, milestone) {
    const storage = getStorageArea();
    return new Promise((res) => {
      if (!storage) {
        res();
        return;
      }
      storage.set({ ["ytTracker_progress_" + id]: milestone }, res);
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
      if (videoId) {
        saveProgressMilestone(videoId, lastReportedProgressMilestone).catch(() => {});
      }
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
    await updateWatchMarker().catch(() => {});

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
  async function attachVideo(v) {
    video = v;
    videoId = getVideoId();
    sessionStart = Date.now();
    sessionWatchedSeconds = 0;
    lastCurrentTime = null;
    lastReportedProgressMilestone = 0;

    if (tickInterval) clearInterval(tickInterval);

    if (videoId) {
      lastReportedProgressMilestone = await loadProgressMilestone(videoId);
    }

    tickInterval = setInterval(tick, 1000);
    await updateWatchMarker().catch(() => {});
  }

  function findAndAttach() {
    const v = document.querySelector("video");
    if (v && v !== video) {
      attachVideo(v);
    }
  }

  function getWatchLikeButton() {
    return document.querySelector(
      'ytd-video-primary-info-renderer #like-button, ytd-video-primary-info-renderer ytd-toggle-button-renderer#like-button, ytd-video-primary-info-renderer ytd-toggle-button-renderer[aria-label*="like"], ytd-video-primary-info-renderer ytd-toggle-button-renderer[aria-label*="Like"]'
    );
  }

  async function updateWatchMarker() {
    if (!videoId) return;
    const likeButton = getWatchLikeButton();
    if (!likeButton) return;

    const marker = await getMarkerForId(videoId);
    const parent = likeButton.parentElement;
    if (!parent) return;

    let markerEl = parent.querySelector('.tkrk-watch-marker');
    if (!markerEl) {
      markerEl = document.createElement('span');
      markerEl.className = 'tkrk-watch-marker';
      markerEl.style.marginRight = '0.5rem';
      markerEl.style.fontWeight = '700';
      markerEl.style.display = 'inline-flex';
      markerEl.style.alignItems = 'center';
      markerEl.style.verticalAlign = 'middle';
      markerEl.style.whiteSpace = 'nowrap';
      markerEl.style.color = '#f1f1f1';
      markerEl.style.fontSize = '1.1rem';
      markerEl.style.lineHeight = '1';
      parent.insertBefore(markerEl, likeButton);
    }

    markerEl.textContent = marker;
  }

  // YouTube is a SPA — watch for navigation
  const observer = new MutationObserver(() => {
    const newId = getVideoId();
    if (newId && newId !== videoId) {
      findAndAttach();
    }
    updateWatchMarker().catch(() => {});
  });
  observer.observe(document.body, { childList: true, subtree: true });

  findAndAttach();

  /* ── overlay previews with a ■ symbol ── */
  function createOverlayStyle() {
    if (document.getElementById("tkrk-preview-overlay-style")) return;
    const style = document.createElement("style");
    style.id = "tkrk-preview-overlay-style";
    style.textContent = `
    .tkrk-preview-overlay { position: absolute; left: 6px; bottom: 6px; top: auto; right: auto; z-index: 9999; color: #fff; background: rgba(0,0,0,0.6); padding: 2px 6px; border-radius: 3px; font-weight: 700; font-size: 12px; line-height: 1; pointer-events: none; }
    `;
    document.head.appendChild(style);
  }

  function getPreviewVideoId(el) {
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

    let marker = "■";
    if (previewId) {
      marker = await getMarkerForId(previewId);
    }

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
