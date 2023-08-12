function createSoccerPlayerPicture(url) {
    let item = document.createElement("div");
    item.classList.add("item");
    let pic = document.createElement("div");
    pic.classList.add("picture");
    item.appendChild(pic);
    let image = document.createElement("img");
    image.src = url;
    pic.appendChild(image);
    return item;
}

function covertPositionNumToString(n) {
    switch (n) {
        case 1:
            return "Forward"
        case 2:
            return "Midfielder"
        case 3:
            return "Defender"
        case 4:
            return "Goalkeeper"
        default:
            return "not-found"
    }
}

function createSoccerPlayerRow(soccerPlayer, isProfile, profileListMode) {
    var newRow = document.createElement("div");
    let fullName = "";
    newRow.classList.add("row");
    for (let i = 0; i < soccerPlayer.length - 1; i++) {
        let item = document.createElement("div");
        item.classList.add("item");
        switch (i) {
            case 0:
                try {
                    item = createSoccerPlayerPicture("https://resources.premierleague.com/premierleague/photos/players/40x40/p" + soccerPlayer[i].replace(".jpg", ".png"));
                }
                catch (error) {
                    item = createSoccerPlayerPicture("Images/Photo-Missing.png");
                }
                break;
            case 1:
                fullName += soccerPlayer[i]
                break;
            case 2:
                fullName += " " + soccerPlayer[i]
                item.classList.add("name");
                item.innerHTML = fullName;
                fullName = "";
                break;
            case 3:
                item.innerHTML = covertPositionNumToString(soccerPlayer[i])
                break;
            case 4:
                item.innerHTML = soccerPlayer[i];
                break;
            case 5:
                item.innerHTML = soccerPlayer[i];
                break;
            case 6:
                item.innerHTML = soccerPlayer[i];
                break;
            default:
                break;
        }
        if (i !== 1) {
            newRow.appendChild(item);
        }
    }
    if (isProfile) {
        newRow.appendChild(buildBusinessButton(profileListMode, soccerPlayer));

    }
    newRow.setAttribute("soccerPlayerId", parseInt(soccerPlayer[7]));
    return newRow;
}



function createUserSoccerPlayerRow(soccerPlayer) {
    var newRow = document.createElement("div");
    let fullName = "";
    newRow.classList.add("row");
    for (let i = 0; i < soccerPlayer.length - 1; i++) {
        let item = document.createElement("div");
        item.classList.add("item");
        switch (i) {
            case 0:
                try {
                    item = createSoccerPlayerPicture("https://resources.premierleague.com/premierleague/photos/players/40x40/p" + soccerPlayer[i].replace(".jpg", ".png"));
                }
                catch (error) {
                    item = createSoccerPlayerPicture("Images/Photo-Missing.png");
                }
                break;
            case 1:
                fullName += soccerPlayer[i]
                break;
            case 2:
                fullName += " " + soccerPlayer[i]
                item.classList.add("name");
                item.innerHTML = fullName;
                fullName = "";
                break;
            case 3:
                item.innerHTML = covertPositionNumToString(soccerPlayer[i])
                break;
            case 4:
                item.innerHTML = soccerPlayer[i];
                break;
            case 5:
                item.innerHTML = soccerPlayer[i];
                break;
            case 6:
                item.innerHTML = soccerPlayer[i];
                break;
            case 7:
                if (soccerPlayer[i] === 1) {
                    item.innerHTML = "Main";
                }
                else {
                    item.innerHTML = "Substitute";
                }
            default:
                break;
        }
        if (i !== 1) {
            newRow.appendChild(item);
        }
    }
    newRow.appendChild(buildBusinessButton("remove", soccerPlayer));
    newRow.setAttribute("soccerPlayerId", parseInt(soccerPlayer[8]));
    return newRow;
}


function updateUserPlayersTable() {
    var userPlayersList = document.querySelector(".user-soccerPlares-list .list-container .list");
    userPlayersList.innerHTML = "";
    const order = ["photo", "firstName", "lastName", "position", "team", "score", "price", "role", "soccerPlayerId"];
    getUserSoccerPlayers()
        .then(soccerPlayers => {
            for (let i = 0; i < soccerPlayers.length; i++) {
                let soccerPlayerArr = order.map(key => soccerPlayers[i][key]);
                console.log(soccerPlayerArr)
                userPlayersList.appendChild(createUserSoccerPlayerRow(soccerPlayerArr));
            }
        })
        .catch(error => {
            console.error(error);
            // Handle the error appropriately if needed
        });
}



function buildAddButton(value, soccerPlayer) {
    var button = document.createElement("input");
    button.setAttribute("value", value);
    button.setAttribute("type", "submit");
    button.addEventListener("click", function () {


        let temp = {
            soccerPlayerId: soccerPlayer[7],
            firstName: soccerPlayer[1],
            lastName: soccerPlayer[2],
            score: soccerPlayer[5],
            team: soccerPlayer[4],
            price: soccerPlayer[6],
            position: soccerPlayer[3],
        }
        if (value === "Add Main") {
            temp.role = 1;
        }
        else {
            temp.role = 2;
        }
        addSoccerPlayer(temp)
            .then(response => {
                if (response.success) {
                    updateUserPlayersTable();
                }
            })
    });
    return button;
}

function buildBusinessButton(buttonMode, soccerPlayer) {
    var item = document.createElement("div");
    item.classList.add("item");
    if (buttonMode === "buy") {
        item.classList.add("buy-as-main-btn")
        item.appendChild(buildAddButton("Add Main", soccerPlayer));
        item.appendChild(buildAddButton("Add Sub", soccerPlayer));
        // item.appendChild(button);
    }
    else {
        var button = document.createElement("input");
        button.setAttribute("value", "Remove");
        button.setAttribute("type", "submit");
        // add event listnere to the buuton
        item.classList.add("remove-btn");

        button.addEventListener("click", function () {
            let temp = {
                soccerPlayerId: soccerPlayer[8],
                firstName: soccerPlayer[1],
                lastName: soccerPlayer[2],
                score: soccerPlayer[5],
                team: soccerPlayer[4],
                price: soccerPlayer[6],
                position: soccerPlayer[3],
                role : soccerPlayer[7]
            }
            removeSoccerPlayer(temp)
                .then(response => {
                    if (response) {
                        var uselessRow = document.querySelector(".row[soccerplayerid=${soccerPlayer[8]}]");
                        uselessRow.remove;
                    }
                })
        })
        item.appendChild(button);
    }

    return item;
}

function createUserRow(user) {
    var newRow = document.createElement("div");
    newRow.classList.add("row");
    let item0 = document.createElement("div");
    item0.classList.add("item");
    item0.classList.add("name");
    item0.innerHTML = user.userName;
    newRow.appendChild(item0);
    let item1 = document.createElement("div");
    item1.classList.add("item");
    item1.innerHTML = user.score;
    newRow.appendChild(item1);
    return newRow;
}

function nextPage(mode, isProfile) {
    var nextBtn = document.getElementById("next");
    nextBtn.addEventListener("click", function () {
        event.preventDefault();
        var currentPage = document.getElementById("current-page");
        var lastPage = document.querySelector("#paginationNums a:last-child");
        if (currentPage.innerHTML !== lastPage.innerHTML) {
            var temp = ++currentPage.innerHTML
            currentPage.innerHTML = temp
            if (mode === "soccerPlayers") {
                filterSoccerPlayers(currentPage.innerHTML).then(response => {
                    if (isProfile) {
                        updateSoccerPlayerList(response.soccerPlayers, true, "buy");
                    }
                    else {
                        updateSoccerPlayerList(response.soccerPlayers);
                    }
                    if (response.pageCount === 0) {
                        updatePagination(1, "auto", 1, mode);
                    }
                    else {
                        updatePagination(response.pageCount, "manual", parseInt(currentPage.innerHTML, 10), mode);
                    }
                })
            }
            else if (mode === "users") {
                getUsers(currentPage.innerHTML).then(response => {
                    updateUserList(response.users);
                    if (response.pageCount === 0) {
                        updatePagination(1, "auto", 1, mode);
                    }
                    else {
                        updatePagination(response.pageCount, "manual", parseInt(currentPage.innerHTML, 10), mode);
                    }
                })
            }
        }
    });
}

function previousPage(mode, isProfile) {
    var previousBtn = document.getElementById("previous");
    previousBtn.addEventListener("click", function () {
        event.preventDefault();
        var currentPage = document.getElementById("current-page");
        if (currentPage.innerHTML !== "1") {
            var temp = --currentPage.innerHTML
            currentPage.innerHTML = temp
            if (mode === "soccerPlayers") {
                filterSoccerPlayers(currentPage.innerHTML).then(response => {
                    if (isProfile) {
                        updateSoccerPlayerList(response.soccerPlayers, true, "buy");
                    }
                    else {
                        updateSoccerPlayerList(response.soccerPlayers);
                    }
                    if (response.pageCount === 0) {
                        updatePagination(1, "auto", null, mode);
                    }
                    else {
                        updatePagination(response.pageCount, "manual", temp, mode);
                    }
                })
            }
            else if (mode === "users") {
                getUsers(currentPage.innerHTML).then(response => {
                    updateUserList(response.users);
                    if (response.pageCount === 0) {
                        updatePagination(1, "auto", null, mode);
                    }
                    else {
                        updatePagination(response.pageCount, "manual", temp, mode);
                    }
                })
            }

        }
    });
}

function paginationNumsActivator(page, mode) {
    page.addEventListener("click", function (event) {
        event.preventDefault();
        var currentPage = document.getElementById("current-page");
        currentPage = page.innerHTML;
        if (mode === "soccerPlayers") {
            filterSoccerPlayers(parseInt(currentPage)).then(response => {
                updateSoccerPlayerList(response.soccerPlayers, true, "buy");
                if (response.pageCount === 0) {
                    updatePagination(1, "auto", null, mode);
                }
                else {
                    updatePagination(response.pageCount, "manual", parseInt(currentPage), mode);
                }
            })
        }
        else if (mode === "users") {
            getUsers(parseInt(currentPage)).then(response => {
                updateUserList(response.users);
                if (response.pageCount === 0) {
                    updatePagination(1, "auto", null, mode);
                }
                else {
                    updatePagination(response.pageCount, "manual", parseInt(currentPage), mode);
                }
            })
        }

    });
}

function updatePagination(pageCount, updateMode, currentPageValue, mode) {
    let currentPage = document.createElement("a");
    currentPage.href = "#";
    currentPage.id = "current-page";
    currentPage.innerHTML = currentPageValue;
    let pagination = document.getElementById("paginationNums");
    pagination.innerHTML = "";
    if (updateMode === "auto") {
        for (let i = 1; i <= pageCount; i++) {
            let tempPage = document.createElement("a");
            tempPage.href = "#";
            if (i > 7 && i !== pageCount) {
                continue;
            }
            if (i === 1) {
                tempPage.id = "current-page";
                tempPage.innerHTML = i;
                pagination.appendChild(tempPage);
                continue;
            }
            if (pageCount > 6 && i === 7 && i !== pageCount) {
                tempPage.innerHTML = ". . .";
                tempPage.classList.add("doted");
                pagination.appendChild(tempPage);
                continue;
            }
            tempPage.innerHTML = i;
            pagination.appendChild(tempPage);
            paginationNumsActivator(tempPage, mode);
        }
    }
    else if (updateMode === "manual") {
        if (currentPageValue > pageCount) {
            currentPageValue = 1;
        }
        for (let i = 1; i <= pageCount; i++) {
            let tempPage = document.createElement("a");
            tempPage.href = "#";
            if (i == currentPageValue - 3 && i !== 1) {
                tempPage.innerHTML = ". . .";
                tempPage.classList.add("doted");
                pagination.appendChild(tempPage);
                continue;
            }

            if ((i < currentPageValue - 3 && i !== 1) || (i > currentPageValue + 3 && i !== pageCount)) {
                continue;
            }

            if (i == currentPageValue + 3 && i !== pageCount) {
                tempPage.innerHTML = ". . .";
                tempPage.classList.add("doted");
                pagination.appendChild(tempPage);
                continue;
            }

            if (i == currentPageValue) {
                tempPage.id = "current-page";
            }

            tempPage.innerHTML = i;
            pagination.appendChild(tempPage);
            paginationNumsActivator(tempPage, mode);
        }
    }

}

function updateSoccerPlayerList(soccerPlayers, isProfile, ProfileListMode = null) {
    const order = ["photo", "firstName", "lastName", "position", "team", "score", "price", "soccerPlayerId"];
    let list = document.getElementById("listOfSoccerPlayers");
    list.innerHTML = "";
    for (let i = 0; i < soccerPlayers.length; i++) {
        let soccerPlayer = order.map(key => soccerPlayers[i][key]);
        list.appendChild(createSoccerPlayerRow(soccerPlayer, isProfile, ProfileListMode));
    }
}

function updateUserList(users) {
    let list = document.getElementById("listOfUsers");
    list.innerHTML = "";
    for (let i = 0; i < users.length; i++) {
        list.appendChild(createUserRow(users[i]));
    }
}

function filter(isProfile) {
    let filterBtn = document.getElementById("FilterButton");
    filterBtn.addEventListener("click", function () {
        let currentPage = document.getElementById("current-page");
        filterSoccerPlayers(currentPage.innerHTML).then(response => {
            if (isProfile) {
                updateSoccerPlayerList(response.soccerPlayers, true, "buy");
            }
            else {
                updateSoccerPlayerList(response.soccerPlayers);
            }
            if (response.pageCount === 0) {
                updatePagination(1, "auto", null, "soccerPlayers");
            }
            else {
                updatePagination(response.pageCount, "auto", null, "soccerPlayers");
            }
        })
    });
}