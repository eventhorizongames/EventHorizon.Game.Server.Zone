(function () {
    document.getElementById("start-connection")
        .addEventListener("click", () => {
            const connection = new signalR.HubConnectionBuilder()
                .withUrl("/playerHub", {
                    accessTokenFactory: () => document.getElementById("access-token").value,
                })
                .configureLogging(signalR.LogLevel.Information)
                .build();
            connection.on("ZoneInfo", (zoneInfo) => {
                console.log("Zone Info: ", zoneInfo);
                document.getElementById("zone-info").innerText = "Zone Info Received: Check Dev Console for info.";
            });
            connection.on("ReceiveAction", (event, eventData) => {
                const action = event + " of: " + eventData;
                const li = document.createElement("li");
                li.textContent = encodedMsg;
                document.getElementById("action-received").appendChild(li);
            });
            connection.start().catch(err => console.error(err.toString()));
        });

    document.getElementById("send-action")
        .addEventListener("click", () => {
            connection
                .invoke("SendMessage", user, message)
                .catch(err => console.error(err.toString()));
        });
})();