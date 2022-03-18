class Fireball {
    constructor(canvasId, parameters, dotnetHelper) {
        this.canvas = document.getElementById(canvasId);
        this.canvas.width = parameters.backbufferWidth;
        this.canvas.height = parameters.backbufferHeight;

        this.gl = this.canvas.getContext('webgl');
        this.gl.viewport(0, 0, this.gl.canvas.width, this.gl.canvas.height);

        this.previousTimestamp = 0;

        this.dotnetGraphics = dotnetHelper;
        window.requestAnimationFrame(this.render.bind(this));
    }

    render(timestamp) {
        const currentTimeStamp = timestamp - this.previousTimestamp;
        this.previousTimestamp = timestamp;

        this.dotnetGraphics.invokeMethodAsync('Render', currentTimeStamp)
            .then(_ => window.requestAnimationFrame(this.render.bind(this)));
    }

    clear(red, green, blue, alpha) {
        this.gl.clearColor(red, green, blue, alpha);
        this.gl.clear(this.gl.COLOR_BUFFER_BIT);
    }

    loadShaderProgram(vertexSource, fragmentSource) {
        var vertexShader = this.compileShader(vertexSource, this.gl.VERTEX_SHADER);
        var fragmentShader = this.compileShader(fragmentSource, this.gl.FRAGMENT_SHADER);

        this.shaderProgram = this.createProgram(vertexShader, fragmentShader);

        this.gl.useProgram(this.shaderProgram);
    }

    compileShader(shaderSource, shaderType) {
        var shader = this.gl.createShader(shaderType);

        this.gl.shaderSource(shader, shaderSource);
        this.gl.compileShader(shader);

        var success = this.gl.getShaderParameter(shader, this.gl.COMPILE_STATUS);

        if (!success) {
            throw "could not compile shader:" + this.gl.getShaderInfoLog(shader);
        }

        return shader;
    }

    createProgram(vertexShader, fragmentShader) {
        var program = this.gl.createProgram();

        this.gl.attachShader(program, vertexShader);
        this.gl.attachShader(program, fragmentShader);

        this.gl.linkProgram(program);

        var success = this.gl.getProgramParameter(program, this.gl.LINK_STATUS);

        if (!success) {
            throw "program failed to link:" + this.gl.getProgramInfoLog(program);
        }

        return program;
    }
}

export function createEngine(canvasId, parameters, dotnetHelper) {
    return new Fireball(canvasId, parameters, dotnetHelper);
}