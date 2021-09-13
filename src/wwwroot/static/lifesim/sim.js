// Crude game of life :-)

const cellSize = 10;

function Cell(context, x, y) {
    this.ctx = context;

    this.x = x;
    this.y = y;

    this.state = false;
    this.nextState = false;


    this.render = () => {
        this.ctx.fillStyle = this.state
            ? "#393939"
            : "#333"

        this.ctx.fillRect(this.x * cellSize, this.y * cellSize, cellSize, cellSize);
    }
}

function SimSurface(context, width, height, randomize) {
    this.cells = [];

    this.rowCount = Math.ceil(width / cellSize);
    this.colCount = Math.ceil(height / cellSize);

    for (r = 0; r < this.rowCount; r++) {
        for (c = 0; c < this.colCount; c++) {
            var cell = new Cell(context, r, c);
            cell.state = randomize && Math.random() > .6;
            this.cells.push(cell);
        }
    }


    this.simulate = () => {
        for (let x = 0; x < this.colCount; x++) {
            for (let y = 0; y < this.rowCount; y++) {
                var liveNeighbors = this.isCellAlive(x - 1, y - 1) + this.isCellAlive(x, y - 1) + this.isCellAlive(x + 1, y - 1) + this.isCellAlive(x - 1, y) + this.isCellAlive(x + 1, y) + this.isCellAlive(x - 1, y + 1) + this.isCellAlive(x, y + 1) + this.isCellAlive(x + 1, y + 1);
                var cellIndex = this.getCellIndexAtCoords(x, y);

                var isAlive = this.cells[cellIndex].state;

                this.cells[cellIndex].nextState = liveNeighbors == 3 || isAlive && liveNeighbors == 2;
            }
        }

        for (c = 0; c < this.cells.length; c++) {
            this.cells[c].state = this.cells[c].nextState;
        }
    }

    this.render = () => {
        for (c = 0; c < this.cells.length; c++) {
            this.cells[c].render();
        }
    }

    this.isCellAlive = (x, y) => {
        if (x < 0 || x >= this.colCount || y < 0 || y >= this.rowCount) {
            return false;
        }

        return this.cells[this.getCellIndexAtCoords(x, y)].state ? 1 : 0;
    }

    this.getCellIndexAtCoords = (x, y) => {
        return x + (y * this.colCount);
    }
}

function Renderer() {
    this.canvas = document.getElementById("simSurface");
    this.context = this.canvas.getContext("2d");

    // Initial seed.
    this.surface = new SimSurface(this.context, this.canvas.width, this.canvas.height, true);

    this.render = () => {
        this.surface.simulate();
        this.surface.render();

        setTimeout(() => {
            requestAnimationFrame(this.render);
        }, 500);
    }

}

const renderer = new Renderer();
requestAnimationFrame(renderer.render);
