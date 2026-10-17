function sendMessage() {
    var userInput = document.getElementById("user-input");
    var message = userInput.value;
    if (message.trim() !== "") {
        var chatBox = document.getElementById("user-chat-box");
        var newMessage = document.createElement("p");
        newMessage.textContent = "You: " + message;
        chatBox.appendChild(newMessage);
        userInput.value = "";
        chatBox.scrollTop = chatBox.scrollHeight;

        fetch('/send_message', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ 'message': message })
        })
        .then(response => response.json())
        .then(data => {
            var serverMessage = document.createElement("p");
            serverMessage.textContent = "Server: " + (data.ai_response || data.received_message || "No response");
            chatBox.appendChild(serverMessage);
            chatBox.scrollTop = chatBox.scrollHeight;
        })
        .catch((error) => {
            console.error('Error:', error);
        });
    }
}

function sendPersona() {
    var personaInput = document.getElementById("persona-input");
    var message = personaInput.value;
    if (message.trim() !== "") {
        var chatBox = document.getElementById("persona-chat-box");
        var newMessage = document.createElement("p");
        newMessage.textContent = "Persona: " + message;
        chatBox.appendChild(newMessage);
        personaInput.value = "";
        chatBox.scrollTop = chatBox.scrollHeight;

        fetch('/send_persona_message', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ 'message': message })
        })
        .then(response => response.json())
        .then(data => {
            var serverMessage = document.createElement("p");
            serverMessage.textContent = "Server: " + (data.persona_response || data.received_message || "No response");
            chatBox.appendChild(serverMessage);
            chatBox.scrollTop = chatBox.scrollHeight;
        })
        .catch((error) => {
            console.error('Error:', error);
        });
    }
}

document.addEventListener("DOMContentLoaded", function() {
    document.getElementById("user-input").addEventListener("keydown", function(event) {
        if (event.key === "Enter") {
            event.preventDefault();
            sendMessage();
        }
    });

    document.getElementById("persona-input").addEventListener("keydown", function(event) {
        if (event.key === "Enter") {
            event.preventDefault();
            sendPersona();
        }
    });
});