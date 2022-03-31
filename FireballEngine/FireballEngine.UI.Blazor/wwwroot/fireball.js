class Fireball {
    constructor(canvasId, parameters, dotnetHelper) {
        this.canvas = document.getElementById(canvasId);
        this.canvas.width = parameters.backbufferWidth;
        this.canvas.height = parameters.backbufferHeight;

        this.gl = this.canvas.getContext('webgl2');
        this.gl.viewport(0, 0, this.gl.canvas.width, this.gl.canvas.height);

        this.pointers = [];

        this.previousTimestamp = 0;

        this.dotnetGraphics = dotnetHelper;
        window.requestAnimationFrame(this.render.bind(this));

        this.canvas.addEventListener('keydown', (event) => {
            const keyName = event.key;

            if (keyName === 'Control') {
                // do not alert when only Control key is pressed.
                console.log(`Key pressed ${keyName}`);
            }

            if (event.ctrlKey) {
                // Even though event.key is not 'Control' (e.g., 'a' is pressed),
                // event.ctrlKey may be true if Ctrl key is pressed at the same time.
                console.log(`Combination of ctrlKey + ${keyName}`);
            } else {
                console.log(`Key pressed ${keyName}`);
            }
        }, false);

        this.canvas.addEventListener('keyup', (event) => {
            const keyName = event.key;

            console.log(`Key ${keyName} released`);            
        }, false);
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

    createVertexArray(baseAddress) {
        const alias = Blazor.platform.readStringField(baseAddress);

        const pointer = {
            id: alias,
            vao: this.gl.createVertexArray()
        };
        
        this.gl.bindVertexArray(pointer.vao);

        this.pointers.push(pointer);
    }

    createArrayBuffer(baseAddress) {
        const alias = Blazor.platform.readStringField(baseAddress);
        const dataEntries = Blazor.platform.readObjectField(baseAddress, 4);
        const dataLength = Blazor.platform.getArrayLength(dataEntries);

        const data = [];

        for (let i = 0; i < dataLength; i++) {
            const entry = Blazor.platform.getArrayEntryPtr(dataEntries, i, 28);

            const colorVertex = Blazor.platform.readStructField(entry);            

            const red = Blazor.platform.readFloatField(colorVertex, 0);
            const green = Blazor.platform.readFloatField(colorVertex, 4);
            const blue = Blazor.platform.readFloatField(colorVertex, 8);
            const alpha = Blazor.platform.readFloatField(colorVertex, 12);

            const x = Blazor.platform.readFloatField(colorVertex, 16);
            const y = Blazor.platform.readFloatField(colorVertex, 20);
            const z = Blazor.platform.readFloatField(colorVertex, 24);

            data.push(red);
            data.push(green);
            data.push(blue);
            data.push(alpha);
            data.push(x);
            data.push(y);
            data.push(z);
        }

        const pointer = this.pointers.filter(p => p.id == alias)[0];

        if (pointer == null) {
            throw `${alias} is does not exist`;
        }

        pointer.vbo = this.gl.createBuffer();

        this.gl.bindBuffer(this.gl.ARRAY_BUFFER, pointer.vbo);
        this.gl.bufferData(this.gl.ARRAY_BUFFER, new Float32Array(data), this.gl.STATIC_DRAW);
    }

    createVertexAttribute(baseAddress) {
        const location = Blazor.platform.readInt16Field(baseAddress, 0);
        const size = Blazor.platform.readInt16Field(baseAddress, 4);
        const structureSize = Blazor.platform.readInt16Field(baseAddress, 8);
        const offset = Blazor.platform.readInt16Field(baseAddress, 12);

        this.gl.vertexAttribPointer(location, size, this.gl.FLOAT, false, structureSize, offset);
        this.gl.enableVertexAttribArray(location);
    }

    drawTriangles(baseAddress) {
        const alias = Blazor.platform.readStringField(baseAddress);
        const offset = Blazor.platform.readInt16Field(baseAddress, 4);
        const count = Blazor.platform.readInt16Field(baseAddress, 8);

        const pointer = this.pointers.filter(p => p.id == alias)[0];

        if (pointer == null) {
            throw `${alias} is does not exist`;
        }

        this.gl.bindVertexArray(pointer.vao);
        this.gl.drawArrays(this.gl.TRIANGLES, offset, count);
    }
}

export function createEngine(canvasId, parameters, dotnetHelper) {
    return new Fireball(canvasId, parameters, dotnetHelper);
}