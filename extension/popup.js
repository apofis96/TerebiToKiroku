// popup.js

/* ── utils ── */
function fmt(seconds) {
  if (!seconds || isNaN(seconds)) return "0:00";
  const s = Math.round(seconds);
  const h = Math.floor(s / 3600);
  const m = Math.floor((s % 3600) / 60);
  const sec = s % 60;
  if (h > 0) return `${h}:${String(m).padStart(2, "0")}:${String(sec).padStart(2, "0")}`;
  return `${m}:${String(sec).padStart(2, "0")}`;
}

function fmtWatched(seconds) {
  if (!seconds) return "0m";
  if (seconds < 60) return `${seconds}s`;
  const m = Math.floor(seconds / 60);
  const h = Math.floor(m / 60);
  if (h > 0) return `${h}h ${m % 60}m`;
  return `${m}m`;
}

function timeAgo(ts) {
  const diff = Date.now() - ts;
  if (diff < 60000) return "just now";
  if (diff < 3600000) return Math.floor(diff / 60000) + "m ago";
  if (diff < 86400000) return Math.floor(diff / 3600000) + "h ago";
  return Math.floor(diff / 86400000) + "d ago";
}

/* ── DOM refs ── */
const noVideo = document.getElementById("no-video");
const videoCard = document.getElementById("video-card");
const videoTitle = document.getElementById("video-title");
const progressFill = document.getElementById("progress-fill");
const progressThumb = document.getElementById("progress-thumb");
const timeCurrent = document.getElementById("time-current");
const timeDuration = document.getElementById("time-duration");
const statWatched = document.getElementById("stat-watched");
const statProgress = document.getElementById("stat-progress");
const statRemaining = document.getElementById("stat-remaining");
const statusDot = document.getElementById("status-dot");
const statusText = document.getElementById("status-text");
const playbackSpeed = document.getElementById("playback-speed");
const historyList = document.getElementById("history-list");
const historyEmpty = document.getElementById("history-empty");
const clearBtn = document.getElementById("clear-history");

/* ── Tabs ── */
document.querySelectorAll(".tab").forEach((tab) => {
  tab.addEventListener("click", () => {
    document.querySelectorAll(".tab").forEach((t) => t.classList.remove("active"));
    document.querySelectorAll(".tab-content").forEach((c) => c.classList.remove("active"));
    tab.classList.add("active");
    document.getElementById("tab-" + tab.dataset.tab).classList.add("active");
    if (tab.dataset.tab === "history") renderHistory();
  });
});

/* ── Update UI ── */
function fmtSpeed(speed) {
  if (speed === undefined || speed === null || isNaN(speed)) return "—";
  const value = Number(speed);
  return `${value.toFixed(2).replace(/\.00$/, "")}x`;
}

function updateNowPlaying(data, record) {
  if (!data) {
    noVideo.style.display = "";
    videoCard.classList.add("hidden");
    return;
  }

  noVideo.style.display = "none";
  videoCard.classList.remove("hidden");

  videoTitle.textContent = data.title || "Unknown video";

  const pct = Math.min(100, Math.round((data.progress || 0) * 100));
  progressFill.style.width = pct + "%";
  progressThumb.style.left = pct + "%";

  timeCurrent.textContent = fmt(data.currentTime);
  timeDuration.textContent = fmt(data.duration);

  const watched = record ? record.watchedSeconds : 0;
  statWatched.textContent = fmtWatched(watched);
  statProgress.textContent = pct + "%";

  const remaining = data.duration - (data.currentTime || 0);
  statRemaining.textContent = remaining > 0 ? fmt(remaining) : "—";

  playbackSpeed.textContent = fmtSpeed(data.playbackRate);

  if (data.playing) {
    statusDot.className = "dot dot-play";
    statusText.textContent = "Playing";
  } else {
    statusDot.className = "dot dot-pause";
    statusText.textContent = "Paused";
  }
}

/* ── History ── */
function getStorageArea() {
  return globalThis.chrome?.storage?.local || globalThis.browser?.storage?.local || null;
}

async function renderHistory() {
  const storage = getStorageArea();
  const history = await new Promise((r) => {
    if (!storage) {
      r([]);
      return;
    }
    storage.get(["ytTracker_history"], (d = {}) => r(d.ytTracker_history || []));
  });

  if (!history.length) {
    historyEmpty.style.display = "";
    historyList.innerHTML = "";
    clearBtn.classList.add("hidden");
    return;
  }

  historyEmpty.style.display = "none";
  clearBtn.classList.remove("hidden");
  historyList.innerHTML = "";

  for (const id of history) {
    const rec = await new Promise((r) => {
      if (!storage) {
        r(null);
        return;
      }
      storage.get(["ytTracker_" + id], (d = {}) => r(d["ytTracker_" + id] || null));
    });
    if (!rec) continue;

    const pct = rec.duration ? Math.min(100, Math.round((rec.lastPosition / rec.duration) * 100)) : 0;

    const li = document.createElement("li");
    li.className = "history-item";
    li.innerHTML = `
      <span class="h-icon">▶</span>
      <div class="h-info">
        <div class="h-title">${escHtml(rec.title || "Unknown")}</div>
        <div class="h-meta">
          <span>${fmtWatched(rec.watchedSeconds)}</span>
          <span>${timeAgo(rec.lastSeen)}</span>
        </div>
      </div>
      <div class="h-bar-wrap">
        <div class="h-bar-track"><div class="h-bar-fill" style="width:${pct}%"></div></div>
        <div class="h-pct">${pct}%</div>
      </div>
    `;
    li.addEventListener("click", () => {
      chrome.tabs.create({ url: `https://www.youtube.com/watch?v=${id}&t=${rec.lastPosition}s` });
    });
    historyList.appendChild(li);
  }
}

function escHtml(s) {
  return s.replace(/&/g,"&amp;").replace(/</g,"&lt;").replace(/>/g,"&gt;").replace(/"/g,"&quot;");
}

clearBtn.addEventListener("click", async () => {
  const storage = getStorageArea();
  const history = await new Promise((r) => {
    if (!storage) {
      r([]);
      return;
    }
    storage.get(["ytTracker_history"], (d = {}) => r(d.ytTracker_history || []));
  });
  const keys = history.map((id) => "ytTracker_" + id).concat(["ytTracker_history"]);
  if (storage) storage.remove(keys, renderHistory);
  else renderHistory();
});

/* ── Init: query active tab ── */
async function init() {
  const [tab] = await chrome.tabs.query({ active: true, currentWindow: true });

  if (!tab || !tab.url || !tab.url.includes("youtube.com/watch")) {
    updateNowPlaying(null);
    return;
  }

  // Ask content script for current state
  chrome.tabs.sendMessage(tab.id, { type: "GET_CURRENT" }, async (response) => {
    if (chrome.runtime.lastError || !response) {
      updateNowPlaying(null);
      return;
    }

    const storage = getStorageArea();
    const record = await new Promise((r) => {
      if (!storage) {
        r(null);
        return;
      }
      storage.get(["ytTracker_" + response.id], (d = {}) => r(d["ytTracker_" + response.id] || null));
    });

    updateNowPlaying(response, record);
  });
}

// Listen for live ticks from content script
chrome.runtime.onMessage.addListener(async (msg) => {
  if (msg.type === "YT_TICK") {
    const storage = getStorageArea();
    const record = await new Promise((r) => {
      if (!storage) {
        r(null);
        return;
      }
      storage.get(["ytTracker_" + msg.data.id], (d = {}) => r(d["ytTracker_" + msg.data.id] || null));
    });
    updateNowPlaying({ ...msg.data, playing: true }, record);
  }
});

init();
