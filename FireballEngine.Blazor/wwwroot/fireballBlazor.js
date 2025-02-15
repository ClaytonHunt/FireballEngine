let _gl = null;
let _shaders = {};
let _currentShader = null;

export function init(dotNetObjRef, containerRef, width, height, title) {
  const container = containerRef;
  const canvas = document.createElement('canvas');
  canvas.width = width;
  canvas.height = height;
  container.appendChild(canvas);

  let gl = canvas.getContext('webgl2') || canvas.getContext('webgl');

  if (!gl) {
    console.error('[Fireball Blazor] WebGL not supported');
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

export function createShader(shaderId, vertexSource, fragmentSource) {
  if (!_gl) {
    console.error('[Fireball Blazor] WebGL not initialized.');
    return null;
  }

  console.log(`[Fireball Blazor] Creating WebGL Shader: ${shaderId}`);

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

  _shaders[shaderId] = shaderProgram; // Store the shader program
  console.log(`[Fireball Blazor] WebGL Shader ${shaderId} created successfully.`);
}

export function useShader(shaderId) {
  if (!_gl || !_shaders[shaderId]) {
    console.error(`[Fireball Blazor] Attempted to use invalid shader: ${shaderId}`);
    return;
  }

  if (_currentShader !== shaderId) {
    _gl.useProgram(_shaders[shaderId]);
    _currentShader = shaderId;
    console.log(`[Fireball Blazor] Switched to shader ${shaderId}`);
  }
}

export function setColor(shaderId, r, g, b, a) {
  if (!_gl || !_shaders[shaderId]) {
    console.error(`[Fireball Blazor] Attempted to set color on invalid shader: ${shaderId}`);
    return;
  }

  const colorLocation = _gl.getUniformLocation(_shaders[shaderId], 'uColor');
  _gl.uniform4f(colorLocation, r, g, b, a);
}

export function setMatrix(shaderId, uniformName, matrix) {
  if (!_gl || !_shaders[shaderId]) {
      console.error(`[Fireball Blazor] Attempted to set matrix on invalid shader: ${shaderId}, Available Shaders: ${Object.keys(_shaders)}`);
      return;
  }

  const location = _gl.getUniformLocation(_shaders[shaderId], uniformName);
  if (!location) {
      console.error(`[Fireball Blazor] Uniform ${uniformName} not found.`);
      return;
  }

  _gl.uniformMatrix4fv(location, false, matrix);
}


export function drawTriangle() {
  if (!_gl || !_currentShader) return;

  const vertices = new Float32Array([
    0.0,  66.7,  0.0,  // Top (450 - 383.3)
    -50.0, -33.3,  0.0,  // Bottom Left (350 - 383.3)
     50.0, -33.3,  0.0   // Bottom Right (450 - 383.3)
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

function checkShaderCompile(shader, type) {
  if (!_gl.getShaderParameter(shader, _gl.COMPILE_STATUS)) {
    const errorLog = _gl.getShaderInfoLog(shader);
    console.error(`[Fireball Blazor] ${type} Shader Compilation Error:`, errorLog);

    throw new Error(`[Fireball Blazor] ${type} Shader Compilation Error: ${errorLog}`);
  }
}

function checkProgramLink(program) {
  if (!_gl.getProgramParameter(program, _gl.LINK_STATUS)) {
    const errorLog = _gl.getProgramInfoLog(program);
    console.error("[Fireball Blazor] Shader Program Linking Error:", errorLog);

    throw new Error(`[Fireball Blazor] Shader Program Linking Error: ${errorLog}`);
  }
}