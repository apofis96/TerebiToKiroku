// background.js — service worker

chrome.runtime.onMessage.addListener((msg) => {
  // Forward tick events to any open popup connections
  // (popup uses chrome.runtime.onMessage too, so no extra relay needed)
});
