let gameId = null;

const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7085/battleshipsHub")
    .configureLogging(signalR.LogLevel.Information)
    .withAutomaticReconnect()
    .build();

connection.onclose(async () => {
    await connectToHub();
});

async function connectToHub() {
    try {
        await connection.start();
        console.log("SignalR Connected.");
        startGame();
    } catch (err) {
        console.log(err);
        setTimeout(connectToHub, 5000);
    }
};

function startGame(){
    connection.invoke("StartGame")
        .then(response => {
            this.gameId = response.gameId
        })
        .catch(err => {
            console.error(err);
        });
}

function tryHit(cell){
    let xValue = parseInt(cell.getAttribute('x'));
    let yValue = parseInt(cell.getAttribute('y'));

    let request = {
        GameId: this.gameId,
        Position: {
            X: xValue,
            Y: yValue,
        }
    };

    connection.invoke("TryHit", request)
        .then(response => {
            handleTryHitResponse(response);
        })
        .catch(err => {
            console.error(err);
        });
}

function handleTryHitResponse(response){
    console.log(response);
    switch (response.hitResult) {
        case "Hit":
            updateReposneElementWithClass(response, "hit");
            break;
        case "Miss":
            updateReposneElementWithClass(response, "miss");
            break;
        case "Sink":
            updateReposneElementWithClass(response, "sink");
            break;
        case "GameOver":
            handleGameFinished();
            break;
    }
}

function updateReposneElementWithClass(response, cssClass) {
    response.positions.forEach(position => {
        const element = document.querySelector(`[x="${position.x}"][y="${position.y}"]`);
        if (element) {
            element.classList.add(cssClass);
        }
    });
}

function setCells() {
    const board = document.querySelector(".board");
    const rows = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J'];

    // Dodajemy etykiety dla kolumn
    for (let k = 0; k <= 10; k++) {
        const label = document.createElement("div");
        label.className = "label";
        
        label.innerText = k === 0 ? "" : rows[k-1]; 
        board.appendChild(label);
    }

    for (let i = 0; i < 10; i++) {
        // Dodajemy etykiety dla wierszy
        const rowLabel = document.createElement("div");
        rowLabel.className = "label";
        rowLabel.innerText = i + 1;
        board.appendChild(rowLabel);

        for (let j = 0; j < 10; j++) {
            const cell = document.createElement("div");
            cell.className = "cell";
            cell.setAttribute('x', j);
            cell.setAttribute('y', i);
            cell.addEventListener('click', function() {
                if (!cell.classList.contains("tried")) {
                    tryHit(cell)
                    cell.classList.add("tried");
                }
            });
            board.appendChild(cell);
        }
    }
}

function handleGameFinished() {
    const board = document.querySelector(".board");
    const legend = document.querySelector(".legend");

    // Ukrywamy planszę i legendę
    board.style.display = "none";
    legend.style.display = "none";

    const congratsMessage = document.createElement("h2");
    congratsMessage.innerText = "Congratulations! You finished the game.";
    document.body.appendChild(congratsMessage);

    const restartButton = document.createElement("button");
    restartButton.innerText = "Start a New Game";
    restartButton.addEventListener('click', function() {
        location.reload(); 
    });
    document.body.appendChild(restartButton);
}

connectToHub();
setCells();