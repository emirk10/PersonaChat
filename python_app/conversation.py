class ChatConversation:
    def __init__(self):
        self.messages = []

    def add_message(self, role, content):
        self.messages.append({"role": role, "content": content})

    def get_messages(self):
        return self.messages

    def clear_messages(self):
        self.messages = []

    def _shrink_chat_history(self, max_length=10):
        """Shrink the chat history to always keep the first message and the last `max_length - 1` messages."""
        # Düzeltme: self.chat_history yerine self.messages kullanmalıyız.
        if len(self.messages) > max_length:
            self.messages = [self.messages[0]] + self.messages[-(max_length - 1):]
        return self.messages # return self.chat_history yerine self.messages

    def summarize_chat(self):
        """Summarize the chat history."""
        if not self.messages:
            return "No messages in chat history."
        
        summary = "Chat Summary:\n"
        for message in self.messages:
            summary += f"{message['role']}: {message['content']}\n"
        return summary