const syncButton = document.getElementById("syncButton");

syncButton.addEventListener("click", async () => {
    try {
        const response = await fetch("/sync", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            }
        });

        if (response.ok) {
            alert("Success");
        }
        else {
            alert("Failure");
        }
    }
    catch (error) {
        console.error("An error ocurred: ", error);
    }
});