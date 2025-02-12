let _gl = null;

export function init(dotNetObjRef, containerRef, width, height, title) {
  const container = containerRef;
  const canvas = document.createElement('canvas');
  canvas.width = width;
  canvas.height = height;
  container.appendChild(canvas);

  let gl = canvas.getContext('webgl2') || canvas.getContext('webgl');

  if (!gl) {
    console.error('WebGL not supported');
    return;
  }

  _gl = gl;
  document.title = title;

  function frameCallback(timestamp) {
    dotNetObjRef.invokeMethodAsync('OnFrame', timestamp);
    requestAnimationFrame(frameCallback);
  }

  requestAnimationFrame(frameCallback);
}

export function setClearColor(r, g, b, a) {
  if (!_gl) {
    return;
  }

  _gl.clearColor(r, g, b, a);
}

export function clearBuffer() {
  if (!_gl) {
    return;
  }

  _gl.clear(_gl.COLOR_BUFFER_BIT | _gl.DEPTH_BUFFER_BIT);
}