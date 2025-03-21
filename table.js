document.addEventListener("DOMContentLoaded", function () {
    let classForm = document.getElementById("class-form");
    let classTableBody = document.querySelector("#class-table tbody");

    // AI Prompt: Add focus and blur events to all form inputs and change background color accordingly.
    let inputs = document.querySelectorAll("#class-form input");
    inputs.forEach(input => {
        input.addEventListener("focus", function () {
            input.style.backgroundColor = "#ffeb3b";
        });
        input.addEventListener("blur", function () {
            input.style.backgroundColor = "";
        });
    });

    // AI Prompt: Prevent default form submission and append a new row to a table using input values.
    classForm.addEventListener("submit", function (event) {
        event.preventDefault();

        let className = document.getElementById("class-name").value;
        let numberPeople = document.getElementById("number-people").value;
        let description = document.getElementById("description").value;

        let newRow = document.createElement("tr");

        newRow.innerHTML = `
            <td>${className}</td>
            <td>${numberPeople}</td>
            <td>${description}</td>
        `;

        // AI Prompt: Log row data to the console when a table row is clicked.
        newRow.addEventListener("click", function () {
            console.log("Row information:", {
                className: className,
                numberPeople: numberPeople,
                description: description
            });
        });

        // AI Prompt: Add a double-click event to remove a table row after confirmation.
        newRow.addEventListener("dblclick", function () {
            let confirmation = confirm("Are you sure you want to delete this row?");
            if (confirmation) {
                newRow.remove();
            }
        });

        // AI Prompt: On mouseover, highlight the row. On mouseout, reset to original background.
        newRow.addEventListener("mouseover", function () {
            newRow.style.backgroundColor = "#ffeb3b";
        });

        newRow.addEventListener("mouseout", function () {
            newRow.style.backgroundColor = "";
        });

        classTableBody.appendChild(newRow);
        classForm.reset();
    });
});
