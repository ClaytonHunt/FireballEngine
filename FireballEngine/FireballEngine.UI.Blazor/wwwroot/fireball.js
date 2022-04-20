class Fireball {
    constructor(canvasId, parameters, dotnetHelper) {
        this.canvas = document.getElementById(canvasId);
        this.canvas.width = parameters.backbufferWidth;
        this.canvas.height = parameters.backbufferHeight;

        this.gl = this.canvas.getContext('webgl2');
        this.gl.viewport(0, 0, this.gl.canvas.width, this.gl.canvas.height);
        this.gl.enable(this.gl.CULL_FACE);
        this.gl.cullFace(this.gl.BACK);
         this.gl.enable(this.gl.DEPTH_TEST);

        this.shaders = [];
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
        this.gl.clear(this.gl.DEPTH_BUFFER_BIT | this.gl.COLOR_BUFFER_BIT);
    }

    loadShaderProgram(alias, vertexSource, fragmentSource) {
        var vertexShader = this.compileShader(vertexSource, this.gl.VERTEX_SHADER);
        var fragmentShader = this.compileShader(fragmentSource, this.gl.FRAGMENT_SHADER);        

        let shaderProgram = {
            id: alias,
            program: this.createProgram(vertexShader, fragmentShader)
        };

        this.shaders.push(shaderProgram);

        this.gl.useProgram(shaderProgram.program);
    }

    setUniformMatrix4(baseAddress) {
        const shaderAlias = Blazor.platform.readStringField(baseAddress, 0);
        const uniformAlias = Blazor.platform.readStringField(baseAddress, 4);
        const matrixEntry = Blazor.platform.readStructField(baseAddress, 8);

        const matrix = [];

        for (let i = 0; i < 16; i++) {
            matrix.push(Blazor.platform.readFloatField(matrixEntry, i * 4));
        }

        const shader = this.shaders.filter(s => s.id == shaderAlias)[0];

        const uniformOffset = this.gl.getUniformLocation(shader.program, uniformAlias);
        this.gl.uniformMatrix4fv(uniformOffset, false, matrix);
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

        const pointer = this.pointers.filter(x => x.id == alias)[0] ?? {};

        pointer.id = alias;
        pointer.vao = this.gl.createVertexArray();
        
        this.gl.bindVertexArray(pointer.vao);        

        this.pointers.push(pointer);
    }

    createArrayBuffer(baseAddress) {
        const alias = Blazor.platform.readStringField(baseAddress);
        const structSize = Blazor.platform.readInt32Field(baseAddress, 4);
        const dataEntries = Blazor.platform.readObjectField(baseAddress, 8);
        const dataLength = Blazor.platform.getArrayLength(dataEntries);

        const data = [];

        for (let i = 0; i < dataLength; i++) {
            const entry = Blazor.platform.getArrayEntryPtr(dataEntries, i, structSize * 4);

            const vertexStruct = Blazor.platform.readStructField(entry);

            for (let j = 0; j < structSize; j++) {                
                data.push(Blazor.platform.readFloatField(vertexStruct, j * 4));
            }            
        }

        const pointer = this.pointers.filter(p => p.id == alias)[0];

        if (pointer == null) {
            throw `${alias} is does not exist`;
        }

        pointer.vbo = this.gl.createBuffer();

        this.gl.bindBuffer(this.gl.ARRAY_BUFFER, pointer.vbo);
        this.gl.bufferData(this.gl.ARRAY_BUFFER, new Float32Array(data), this.gl.STATIC_DRAW);
    }

    createElementArrayBuffer(baseAddress) {
        const alias = Blazor.platform.readStringField(baseAddress);
        const dataEntries = Blazor.platform.readObjectField(baseAddress, 4);
        const dataLength = Blazor.platform.getArrayLength(dataEntries);

        const data = [];

        for (let i = 0; i < dataLength; i++) {
            const entry = Blazor.platform.getArrayEntryPtr(dataEntries, i, 4);

            const position = Blazor.platform.readInt16Field(entry);

            data.push(position);
        }

        const pointer = this.pointers.filter(p => p.id == alias)[0];

        if (pointer == null) {
            throw `${alias} is does not exist`;
        }

        pointer.ebo = this.gl.createBuffer();

        this.gl.bindBuffer(this.gl.ELEMENT_ARRAY_BUFFER, pointer.ebo);
        this.gl.bufferData(this.gl.ELEMENT_ARRAY_BUFFER, new Uint16Array(data), this.gl.STATIC_DRAW);
    }

    createTexture(baseAddress) {
        const alias = Blazor.platform.readStringField(baseAddress, 0);
        const imageSrc = Blazor.platform.readStringField(baseAddress, 4);

        const pointer = this.pointers.filter(p => p.id == alias)[0];

        if (pointer == null) {
            throw `${alias} is does not exist`;
        }        

        const loadImage = () => new Promise(resolve => {
            const image = new Image();
            image.addEventListener('load', () => resolve(image));
            image.src = imageSrc;
        });

        const run = async () => {
            const image = await loadImage();

            this.gl.pixelStorei(this.gl.UNPACK_FLIP_Y_WEBGL, true);

            pointer.texture = this.gl.createTexture();
            this.gl.bindTexture(this.gl.TEXTURE_2D, pointer.texture);            
            this.gl.texImage2D(this.gl.TEXTURE_2D, 0, this.gl.RGBA, this.gl.RGBA, this.gl.UNSIGNED_BYTE, image);
            this.gl.generateMipmap(this.gl.TEXTURE_2D);
            this.gl.texParameteri(this.gl.TEXTURE_2D, this.gl.TEXTURE_WRAP_S, this.gl.REPEAT);
            this.gl.texParameteri(this.gl.TEXTURE_2D, this.gl.TEXTURE_WRAP_T, this.gl.REPEAT);
            this.gl.texParameteri(this.gl.TEXTURE_2D, this.gl.TEXTURE_MIN_FILTER, this.gl.NEAREST);
            this.gl.texParameteri(this.gl.TEXTURE_2D, this.gl.TEXTURE_MAG_FILTER, this.gl.NEAREST);
        };

        run();
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

    drawLines(baseAddress) {
        const alias = Blazor.platform.readStringField(baseAddress);
        const offset = Blazor.platform.readInt16Field(baseAddress, 4);
        const count = Blazor.platform.readInt16Field(baseAddress, 8);

        const pointer = this.pointers.filter(p => p.id == alias)[0];

        if (pointer == null) {
            throw `${alias} is does not exist`;
        }

        this.gl.bindVertexArray(pointer.vao);
        this.gl.drawArrays(this.gl.LINES, offset, count);
    }

    drawElement(baseAddress) {
        const alias = Blazor.platform.readStringField(baseAddress);
        const offset = Blazor.platform.readInt16Field(baseAddress, 4);
        const count = Blazor.platform.readInt16Field(baseAddress, 8);

        const pointer = this.pointers.filter(p => p.id == alias)[0];

        if (pointer == null) {
            throw `${alias} is does not exist`;
        }

        this.gl.bindVertexArray(pointer.vao);

        if (pointer.texture) {
            this.gl.bindTexture(this.gl.TEXTURE_2D, pointer.texture);
        }

        this.gl.drawElements(this.gl.TRIANGLES, count, this.gl.UNSIGNED_SHORT, offset);
    }

    drawShapeOutline(baseAddress) {
        const alias = Blazor.platform.readStringField(baseAddress);
        const offset = Blazor.platform.readInt16Field(baseAddress, 4);
        const count = Blazor.platform.readInt16Field(baseAddress, 8);

        const pointer = this.pointers.filter(p => p.id == alias)[0];

        if (pointer == null) {
            throw `${alias} is does not exist`;
        }

        this.gl.bindVertexArray(pointer.vao);
        this.gl.drawElements(this.gl.LINES, count, this.gl.UNSIGNED_INT, offset);
    }
}

export function createEngine(canvasId, parameters, dotnetHelper) {
    return new Fireball(canvasId, parameters, dotnetHelper);
}