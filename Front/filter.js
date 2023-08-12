
// Range Filters Beginning
const priceRangeInput = document.querySelectorAll(".priceInputs input"),
    priceInput = document.querySelectorAll(".priceNumInput input"),
    priceRange = document.querySelector(".slider .priceProgress"),

    scoreRangeInput = document.querySelectorAll(".scoreInputs input"),
    scoreInput = document.querySelectorAll(".scoreNumInput input"),
    scoreRange = document.querySelector(".slider .scoreProgress");
let priceGap = 1;
let scoreGap = 5;
priceInput.forEach(input => {
    input.addEventListener("input", e => {
        let minPrice = parseInt(priceInput[0].value),
            maxPrice = parseInt(priceInput[1].value);

        if ((maxPrice - minPrice >= priceGap) && maxPrice <= priceRangeInput[1].max) {
            if (e.target.className === "input-min") {
                priceRangeInput[0].value = minPrice;
                priceRange.style.left = ((minPrice / priceRangeInput[0].max) * 100) + "%";
            } else {
                priceRangeInput[1].value = maxPrice;
                priceRange.style.right = 100 - (maxPrice / priceRangeInput[1].max) * 100 + "%";
            }
        }
    });
});

priceRangeInput.forEach(input => {
    input.addEventListener("input", e => {
        let minVal = parseInt(priceRangeInput[0].value),
            maxVal = parseInt(priceRangeInput[1].value);

        if ((maxVal - minVal) < priceGap) {
            if (e.target.className === "range-min") {
                priceRangeInput[0].value = maxVal - priceGap
            } else {
                priceRangeInput[1].value = minVal + priceGap;
            }
        } else {
            priceInput[0].value = minVal;
            priceInput[1].value = maxVal;
            priceRange.style.left = ((minVal / priceRangeInput[0].max) * 100) + "%";
            priceRange.style.right = 100 - (maxVal / priceRangeInput[1].max) * 100 + "%";
        }
    });
});

scoreInput.forEach(input => {
    input.addEventListener("input", e => {
        let minPrice = parseInt(scoreInput[0].value),
            maxPrice = parseInt(scoreInput[1].value);

        if ((maxPrice - minPrice >= scoreGap) && maxPrice <= scoreRangeInput[1].max) {
            if (e.target.className === "input-min") {
                scoreRangeInput[0].value = minPrice;
                scoreRange.style.left = ((minPrice / scoreRangeInput[0].max) * 100) + "%";
            } else {
                scoreRangeInput[1].value = maxPrice;
                scoreRange.style.right = 100 - (maxPrice / scoreRangeInput[1].max) * 100 + "%";
            }
        }
    });
});

scoreRangeInput.forEach(input => {
    input.addEventListener("input", e => {
        let minVal = parseInt(scoreRangeInput[0].value),
            maxVal = parseInt(scoreRangeInput[1].value);

        if ((maxVal - minVal) < scoreGap) {
            if (e.target.className === "range-min") {
                scoreRangeInput[0].value = maxVal - scoreGap
            } else {
                scoreRangeInput[1].value = minVal + scoreGap;
            }
        } else {
            scoreInput[0].value = minVal;
            scoreInput[1].value = maxVal;
            scoreRange.style.left = ((minVal / scoreRangeInput[0].max) * 100) + "%";
            scoreRange.style.right = 100 - (maxVal / scoreRangeInput[1].max) * 100 + "%";
        }
    });
});
// Range Filters Ending


// Filter Button In Mobile Beginning

function toggleFilterBar() {
    let filterSide = document.getElementsByClassName("filter-side")[0];
    event.preventDefault();
    let computedStyle = window.getComputedStyle(filterSide);
    let visibility = computedStyle.visibility;
    if (visibility === "hidden") {
        filterSide.classList.add("enable-filter-side");
    }
    else {
        filterSide.classList.remove("enable-filter-side");
    }
}

// Filter Button In Mobile Ending


//activating position, Order of, order type  inputs

{
    let positions = document.querySelectorAll('.positions input[type="submit"]');
    let orders = document.querySelectorAll('.order-of input[type="submit"]');
    let orderTypes = document.querySelectorAll('.order-type input[type="submit"]');
    const all = {
        positions: positions,
        orders: orders,
        orderTypes: orderTypes
    }
    function deactivateOthers(n, arr) {
        for (let i = 0; i < arr.length; i++) {
            if (i !== n) {
                arr[i].setAttribute("active", "0");
                arr[i].id = "";
            }
        }
    }
    for (const listName in all) {
        const list = all[listName];
        for (let i = 0; i < list.length; i++) {
            list[i].addEventListener("click", function () {
                deactivateOthers(i, list);
                list[i].setAttribute("active", "1");
                list[i].id = "active-position";
            })
        }
    }
}

