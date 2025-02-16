let _gl = null;
let _shaders = {};
let _currentShader = null;
let _textures = {};
let _dotNetObjRef = null;

const fire = {
  info: (message) => _dotNetObjRef.invokeMethodAsync("LogMessage", 'info', message),
  warn: (message) => _dotNetObjRef.invokeMethodAsync("LogMessage", 'warn', message),
  error: (message) => _dotNetObjRef.invokeMethodAsync("LogMessage", 'error', message)
};

export function init(dotNetObjRef, containerRef, width, height, title) {
  _dotNetObjRef = dotNetObjRef;
  const container = containerRef;
  const canvas = document.createElement('canvas');
  canvas.width = width;
  canvas.height = height;
  container.appendChild(canvas);

  let gl = canvas.getContext('webgl2') || canvas.getContext('webgl');

  if (!gl) {
    fire.error('WebGL not supported');
    return;
  }

  _gl = gl;
  document.title = title;

  _gl.enable(_gl.BLEND);
  _gl.blendFuncSeparate(_gl.SRC_ALPHA, _gl.ONE_MINUS_SRC_ALPHA, _gl.ONE, _gl.ONE_MINUS_SRC_ALPHA);
}

export function start() {
  function frameCallback(timestamp) {
    _dotNetObjRef.invokeMethodAsync('OnFrame', timestamp);
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

export function createShader(shaderId, vertexSource, fragmentSource) {
  if (!_gl) {
    fire.error('Cannot create Shader: WebGL not initialized.');
    return null;
  }

  const vertexShader = _gl.createShader(_gl.VERTEX_SHADER);
  _gl.shaderSource(vertexShader, vertexSource);
  _gl.compileShader(vertexShader);
  checkShaderCompile(vertexShader, 'VERTEX');

  const fragmentShader = _gl.createShader(_gl.FRAGMENT_SHADER);
  _gl.shaderSource(fragmentShader, fragmentSource);
  _gl.compileShader(fragmentShader);
  checkShaderCompile(fragmentShader, 'FRAGMENT');

  const shaderProgram = _gl.createProgram();
  _gl.attachShader(shaderProgram, vertexShader);
  _gl.attachShader(shaderProgram, fragmentShader);
  _gl.linkProgram(shaderProgram);
  checkProgramLink(shaderProgram);

  _shaders[shaderId] = shaderProgram;
}

export function useShader(shaderId) {
  if (!_gl || !_shaders[shaderId]) {
    fire.error(`Attempted to use invalid shader: ${shaderId}`);
    return;
  }

  if (_currentShader !== shaderId) {
    _gl.useProgram(_shaders[shaderId]);
    _currentShader = shaderId;
  }
}

export function setColor(shaderId, r, g, b, a) {
  if (!_gl || !_shaders[shaderId]) {
    fire.error(`Attempted to set color on invalid shader: ${shaderId}`);
    return;
  }

  const colorLocation = _gl.getUniformLocation(_shaders[shaderId], 'uColor');
  _gl.uniform4f(colorLocation, r, g, b, a);
}

export function setMatrix(shaderId, uniformName, matrix) {
  if (!_gl || !_shaders[shaderId]) {
    fire.error(`Attempted to set matrix on invalid shader: ${shaderId}`);
    return;
  }

  const location = _gl.getUniformLocation(_shaders[shaderId], uniformName);
  if (!location) {
    fire.error(`Uniform ${uniformName} not found.`);
    return;
  }

  _gl.uniformMatrix4fv(location, false, matrix);
}

export async function preloadTexture(id, src) {
  let imageLodaed = new Promise((resolve, reject) => {
    let img = new Image();
    img.src = src;
    img.onload = () => {
      loadTexture(id, img);
      resolve(img);
    };
    img.onerror = () => reject(new Error(`Failed to load image: ${src}`));
  });

  await imageLodaed;
}

export function loadTexture(id, img) {
  if (!_gl) {
    fire.error('Cannot load texture: WebGL not initialized.');
    return;
  }

  if (_textures.hasOwnProperty(id)) {
    fire.error(`Texture with ID ${id} already loaded.`);
    return;
  }

  const texture = _gl.createTexture();
  _gl.bindTexture(_gl.TEXTURE_2D, texture);
  _gl.pixelStorei(_gl.UNPACK_FLIP_Y_WEBGL, true);
  _gl.texImage2D(_gl.TEXTURE_2D, 0, _gl.RGBA, _gl.RGBA, _gl.UNSIGNED_BYTE, img);
  _gl.texParameteri(_gl.TEXTURE_2D, _gl.TEXTURE_MIN_FILTER, _gl.LINEAR);
  _gl.texParameteri(_gl.TEXTURE_2D, _gl.TEXTURE_MAG_FILTER, _gl.LINEAR);
  _gl.generateMipmap(_gl.TEXTURE_2D);

  _textures[id] = texture;
}

export function isTextureLoaded(id) {
  return _textures.hasOwnProperty(id);
}

export function getTexture(id) {
  return _textures[id];
}

export function drawTriangle() {
  if (!_gl || !_currentShader) return;

  const vertices = new Float32Array([
    0.0, 66.7, 0.0,  // Top (450 - 383.3)
    -50.0, -33.3, 0.0,  // Bottom Left (350 - 383.3)
    50.0, -33.3, 0.0   // Bottom Right (450 - 383.3)
  ]);

  const vao = _gl.createVertexArray();
  const vbo = _gl.createBuffer();

  _gl.bindVertexArray(vao);
  _gl.bindBuffer(_gl.ARRAY_BUFFER, vbo);
  _gl.bufferData(_gl.ARRAY_BUFFER, vertices, _gl.STATIC_DRAW);

  const positionAttribLocation = _gl.getAttribLocation(_shaders[_currentShader], "aPos");
  if (positionAttribLocation === -1) {
    console.error("[Fireball Blazor] Attribute 'aPos' not found in shader.");
    return;
  }

  _gl.enableVertexAttribArray(positionAttribLocation);
  _gl.vertexAttribPointer(positionAttribLocation, 3, _gl.FLOAT, false, 0, 0);

  _gl.drawArrays(_gl.TRIANGLES, 0, 3);
}

export function drawSprite(textureId, x, y, width, height, originX, originY) {
  if (!_gl || !_currentShader) return;

  // Ensure texture is loaded
  if (!_textures[textureId]) {
    fire.error(`Texture '${textureId}' not loaded.`);
    fire.info("Available Textures: ", Object.keys(_textures));
    return;
  }

  // Adjust position based on origin
  let offsetX = width * originX;
  let offsetY = height * originY;

  // Define the quad vertices in screen space (x, y, u, v)
  // Define the quad vertices with origin adjustment
  const vertices = new Float32Array([
    x - offsetX, y + height - offsetY, 0.0, 0.0, 1.0,  // Top-left
    x - offsetX, y - offsetY, 0.0, 0.0, 0.0,  // Bottom-left
    x + width - offsetX, y - offsetY, 0.0, 1.0, 0.0,  // Bottom-right

    x - offsetX, y + height - offsetY, 0.0, 0.0, 1.0,  // Top-left
    x + width - offsetX, y - offsetY, 0.0, 1.0, 0.0,  // Bottom-right
    x + width - offsetX, y + height - offsetY, 0.0, 1.0, 1.0   // Top-right
  ]);

  const vao = _gl.createVertexArray();
  const vbo = _gl.createBuffer();

  _gl.bindVertexArray(vao);
  _gl.bindBuffer(_gl.ARRAY_BUFFER, vbo);
  _gl.bufferData(_gl.ARRAY_BUFFER, vertices, _gl.STATIC_DRAW);

  const positionAttrib = _gl.getAttribLocation(_shaders[_currentShader], "aPos");
  _gl.vertexAttribPointer(positionAttrib, 3, _gl.FLOAT, false, 5 * 4, 0);
  _gl.enableVertexAttribArray(positionAttrib);

  const texAttrib = _gl.getAttribLocation(_shaders[_currentShader], "aTexCoord");
  _gl.vertexAttribPointer(texAttrib, 2, _gl.FLOAT, false, 5 * 4, 3 * 4);
  _gl.enableVertexAttribArray(texAttrib);

  // Bind texture
  _gl.activeTexture(_gl.TEXTURE0);
  _gl.bindTexture(_gl.TEXTURE_2D, _textures[textureId]);

  // Draw the quad (two triangles)
  _gl.drawArrays(_gl.TRIANGLES, 0, 6);
}

function checkShaderCompile(shader, type) {
  if (!_gl.getShaderParameter(shader, _gl.COMPILE_STATUS)) {
    const errorLog = _gl.getShaderInfoLog(shader);
    fire.error(`${type} Shader Compilation Error:`, errorLog);

    throw new Error(`[Fireball Blazor] ${type} Shader Compilation Error: ${errorLog}`);
  }
}

function checkProgramLink(program) {
  if (!_gl.getProgramParameter(program, _gl.LINK_STATUS)) {
    const errorLog = _gl.getProgramInfoLog(program);
    fire.error("Shader Program Linking Error:", errorLog);

    throw new Error(`[Fireball Blazor] Shader Program Linking Error: ${errorLog}`);
  }
}