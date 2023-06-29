function goBack() {
    history.go(-1);
}

function showChildren(element) {
    var parentLi = element.parentNode; // Pobierz rodzica li
    var childrenUl = parentLi.querySelector("ul"); // Znajdź pierwsze ul wewnątrz rodzica

    if (childrenUl) {
        parentLi.classList.toggle("collapsed"); // Dodaj lub usuń klasę collapsed dla rodzica
    }
}

function toggleAllNodes(button) {
    var nodes = document.getElementsByClassName("tree-node"); // Pobierz wszystkie elementy o klasie "tree-node"
    var toggleButton = document.querySelector("button");
    var iElement = document.getElementById("folderImg")
    var toggleText = button.querySelector(".toggle-text");

    var isExpandAll = toggleButton.textContent.trim() === "Rozwiń wszystkie";

    for (var i = 0; i < nodes.length; i++) { // Dla każdego elementu
        var parentLi = nodes[i].parentNode; // Pobierz rodzica li
        var childrenUl = parentLi.querySelector("ul");

        if (childrenUl) {
            if (isExpandAll) {
                parentLi.classList.remove("collapsed"); // Usuń klasę "collapsed", aby rozwinąć zwinęty węzeł
            } else {
                parentLi.classList.add("collapsed"); // Dodaj klasę "collapsed", aby zwinąć rozsunięty węzeł
            }
        }
    }
    if (isExpandAll) {
        toggleText.textContent = "Schowaj wszystkie";
        iElement.classList.remove("fa-folder-open");
        iElement.classList.add("fa-folder");
    } else {
        toggleText.textContent = "Rozwiń wszystkie";
        iElement.classList.remove("fa-folder");
        iElement.classList.add("fa-folder-open");
    }
}


// Potrzebne zmienne
var contextMenu = document.getElementById("context-menu");
var treeNodes = document.querySelectorAll(".tree-node");

function hideContextMenu(event) {
    var target = event.target;

    // Sprawdź, czy kliknięty element jest węzłem drzewa lub menu kontekstowym
    var isTreeNode = target.classList.contains("tree-node");
    var isContextMenu = target.classList.contains("context-menu");

    if (!isTreeNode && !isContextMenu) {
        contextMenu.style.display = "none"; // Schowaj menu kontekstowe
    }
}
document.addEventListener("click", hideContextMenu); // Nasłuchuj zdarzenia

function showContextMenu(event) {
    event.preventDefault(); // Zatrzymaj domyślne menu kontekstowe przeglądarki

    if (contextMenu.style.display === "block") {
        contextMenu.style.display = "none"; // Schowaj menu kontekstowe, jeśli jest widoczne
    } else {
        var posX = event.clientX + window.scrollX; // Dodaj przesunięcie poziome przewijania strony
        var posY = event.clientY + window.scrollY; // Dodaj przesunięcie pionowe przewijania strony
        var node = event.target; // Pobierz kliknięty element
        var nodeRect = node.getBoundingClientRect(); // Pobierz prostokąt klikniętego elementu
        var nodeRight = nodeRect.right + window.scrollX; // Prawa krawędź klikniętego elementu

        contextMenu.style.left = nodeRight + "px";
        contextMenu.style.top = posY + "px";
        contextMenu.style.display = "block"; // Pokaż menu kontekstowe

        // Przekaż informacje o klikniętym elemencie do menu kontekstowego
        contextMenu.dataset.nodeId = node.dataset.nodeId;
    }
}

function createNode(event) {
    var nodeId = event.target.parentNode.parentNode.dataset.nodeId;
    var nodeName = prompt("Wprowadź nazwę nowego węzła:");

    if (nodeName && nodeName.trim() !== "") {
        var url = 'CreateFromContextMenu/' + nodeId + '?nodeName=' + encodeURIComponent(nodeName); // Kodowanie nazwy

        location.href = url; // Przekierowanie do akcji do kontrolera
    } else {
        alert("Nazwa węzła nie może być pusta."); // Wyświetlenie komunikatu o błędzie
    }
}

// Pobieranie wartości klikniętego node'a i przekazywanie odpowiednich atrybutów do funkcji
function editNode(event) {
    var nodeId = event.target.parentNode.parentNode.dataset.nodeId;
    location.href = 'TreeNode/Edit/' + nodeId;
}

function deleteNode(event) {
    var nodeId = event.target.parentNode.parentNode.dataset.nodeId;
    if (confirm('Czy na pewno chcesz usunąć?')) {
        location.href = 'TreeNode/Delete/' + nodeId;
    }
}

function addNode(event) {
    location.href = 'TreeNode/Create';
}

// Przechwytywanie zdarzenia dragstart
function handleDragStart(event) {
    // Pobierz identyfikator węzła, który złapałem
    var nodeId = event.target.getAttribute("data-node-id");

    // Ustaw dane przeciągane (w tym przypadku identyfikator węzła)
    event.dataTransfer.setData("text/plain", nodeId); //Tekst prosty
}

// Dodaj obsługę zdarzenia dragstart dla elementów <span class="tree-node">
var treeNodes = document.querySelectorAll("span.tree-node");
treeNodes.forEach(function (node) {
    node.addEventListener("dragstart", handleDragStart);
});

// Przechwytywanie zdarzenia dragover
function handleDragOver(event) {
    // Zapobiega upuszczaniu elementów
    event.preventDefault();

    // Dodaj klasę wskazującą możliwość upuszczenia
    event.target.classList.add("drag-over");
}

// Przechwytywanie zdarzenia dragleave gdy opuszczamy <span>
function handleDragLeave(event) {
    // Usuń klasę wskazującą możliwość upuszczenia
    event.target.classList.remove("drag-over");
}

// Dodaj obsługę zdarzenia dragover i dragleave dla elementów <span class="tree-node">
var treeNodes = document.querySelectorAll("span.tree-node");
treeNodes.forEach(function (node) {
    node.addEventListener("dragover", handleDragOver);
    node.addEventListener("dragleave", handleDragLeave);
});

// Obsługa zdarzenia drop dla elementów <span>
function handleDrop(event) {
    event.preventDefault();

    // Odczytanie danych przeciąganych (id)
    var nodeId = event.dataTransfer.getData("text/plain");

    // Wykonanie odpowiednich akcji na podstawie odczytanych danych
    var targetNodeId = event.target.getAttribute("data-node-id");
    // Wywołanie akcji z kontrolera i przekazanie atrybutów
    console.log(nodeId);
    console.log(targetNodeId);
    clearDragStyles();
    location.href = '/TreeNode/MoveNode/?nodeId=' + nodeId + '&targetNodeId=' + targetNodeId;
    clearDragStyles();
}

// Przypisanie obsługi zdarzenia drop dla elementów <span class="tree-node">
var treeNodes = document.querySelectorAll(".tree-node");
treeNodes.forEach(function (node) {
    node.addEventListener("drop", handleDrop);
});

function clearDragStyles() {
    var elements = document.querySelectorAll('.tree-node');
    elements.forEach(function (element) {
        element.classList.remove('drag-over');
    });
}
