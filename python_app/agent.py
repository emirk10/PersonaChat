from conversation import ChatConversation
from flask import current_app
from typing import Optional
from langchain_google_genai import ChatGoogleGenerativeAI

class Persona:
    """Represents the agent's role/personality."""
    def __init__(self, description: Optional[str] = None):
        self.description = description or "You are a helpful assistant."

    def set(self, new_description: str) -> str:
        """Update the persona description."""
        self.description = new_description
        return f"Persona set to: {new_description}"

    def reset(self) -> str:
        """Reset to default persona."""
        self.description = "You are a helpful assistant."
        return "Persona reset to default."

class Agent(ChatConversation):
    def __init__(self, llm, persona: Optional[Persona] = None):
        super().__init__()
        self.llm = llm
        self.persona = persona or Persona()

    def process_message(self, message: str) -> str:
        """
        Process user input with the current persona, maintaining chat history.
        The persona instruction is prepended to the message list sent to the LLM.
        """
        # --- Önceki clear_messages() kaldırıldı ---
        # Artık sohbet geçmişini her mesajda temizlemiyoruz.
        
        # Kullanıcının yeni mesajını sohbet geçmişine ekle
        self.add_message(role="user", content=message)

        # LLM'e gönderilecek mesaj listesini oluştur
        # Persona talimatını her zaman mesaj listesinin en başına yerleştir
        messages_to_send = []

        # Persona talimatını LLM'e bir sistem mesajı gibi ilet
        persona_instruction = self.persona.description
        messages_to_send.append({"role": "user", "content": f"Sistem talimatı: {persona_instruction}"})

        # Mevcut sohbet geçmişindeki tüm mesajları messages_to_send listesine ekle
        # Bu, LLM'in önceki konuşmaları hatırlamasını sağlar.
        for msg in self.get_messages():
            # Kendi eklediğimiz sistem talimatını tekrar eklememek için kontrol
            if not (msg['role'] == 'user' and msg['content'].startswith("Sistem talimatı:")):
                messages_to_send.append(msg)
        
        current_app.logger.info(f"LLM'e gönderilen son mesaj listesi: {messages_to_send}")

        # LLM'i çağır
        response = self.llm.invoke(messages_to_send)
        
        # LLM'in cevabını geçmişe kaydet
        self.add_message(role="assistant", content=response.content)
        
        return response.content
    
    def process_persona(self, persona_message: str) -> str:
        """
        Process a message related to the agent's persona.
        This method will set the persona and inform the user, AND also try to
        make the LLM acknowledge the new persona.
        """
        if persona_message:
            self._set_persona(persona_message)
            
            # Persona güncellendiğinde LLM'e de bu değişikliği bildirin
            # Bu, LLM'in yeni persona'yı hemen benimsemesine yardımcı olabilir.
            # Sohbet geçmişini temizlediğimiz için, LLM'in persona onayını sıfırdan yapmasını sağlayalım.
            messages_for_llm_ack = []
            ack_persona_instruction = self.persona.description
            messages_for_llm_ack.append({"role": "user", "content": f"Sistem talimatı: {ack_persona_instruction}\n\nKullanıcı: Şu andan itibaren bu kişiliği benimse ve buna göre davran."})
            
            try:
                ack_response = self.llm.invoke(messages_for_llm_ack)
                current_app.logger.info(f"LLM persona onay yanıtı: {ack_response.content}")
                return f"Persona '{self.persona.description}' olarak ayarlandı. Onay: {ack_response.content[:50]}..."
            except Exception as e:
                current_app.logger.error(f"Persona onayında LLM hatası: {e}")
                return f"Persona '{self.persona.description}' olarak ayarlandı, ancak onay alınamadı: {e}"
        return "Persona mesajı boş."

    def _set_persona(self, persona_message: str):
        # Persona değiştiğinde eski sohbet geçmişini temizle
        # Bu, yeni persona ile yeni bir bağlam başlamasını sağlar.
        self.clear_messages() 
        current_app.logger.info(f"Persona güncellendi: {persona_message}")
        self.persona.set(persona_message)