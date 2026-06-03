(function () {
    "use strict";

    const chatMessageForm = document.getElementById("chatMessageForm");
    const chatMessageInput = document.getElementById("chatMessageInput");
    const chatMessages = document.getElementById("chatMessages");

    function scrollChatToBottom() {
        if (chatMessages) {
            chatMessages.scrollTop = chatMessages.scrollHeight;
        }
    }

    function escapeHtml(value) {
        return String(value || "")
            .replaceAll("&", "&amp;")
            .replaceAll("<", "&lt;")
            .replaceAll(">", "&gt;")
            .replaceAll('"', "&quot;")
            .replaceAll("'", "&#039;");
    }

    async function sendChatMessage() {
        if (!chatMessageForm || !chatMessageInput || !chatMessages) {
            return;
        }

        const messageText = chatMessageInput.value.trim();

        if (messageText.length === 0) {
            return;
        }

        const formData = new FormData(chatMessageForm);
        chatMessageInput.value = "";

        try {
            const response = await fetch(chatMessageForm.action, {
                method: "POST",
                body: formData
            });

            const result = await response.json();

            if (!result.success) {
                alert(result.message || "Не удалось отправить сообщение");
                chatMessageInput.value = messageText;
                return;
            }

            const emptyDialog = chatMessages.querySelector(".chat-empty-dialog");

            if (emptyDialog) {
                emptyDialog.remove();
            }

            chatMessages.insertAdjacentHTML("beforeend", `
                <div class="chat-message-row mine">
                    <div class="chat-message-bubble">
                        <div class="chat-message-text">${escapeHtml(result.text)}</div>
                        <div class="chat-message-date">${escapeHtml(result.createdAt)}</div>
                    </div>
                </div>
            `);

            scrollChatToBottom();
        } catch (error) {
            alert("Ошибка при отправке сообщения");
            chatMessageInput.value = messageText;
        }
    }

    if (chatMessageForm && chatMessageInput) {
        chatMessageForm.addEventListener("submit", function (event) {
            event.preventDefault();
            sendChatMessage();
        });

        chatMessageInput.addEventListener("keydown", function (event) {
            if (event.key === "Enter" && !event.shiftKey) {
                event.preventDefault();
                sendChatMessage();
            }
        });
    }

    scrollChatToBottom();
})();
