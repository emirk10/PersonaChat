from flask import request, jsonify
from app import app, agent 
from flask import current_app 

@app.route('/send_message', methods=['POST'])
def send_message():
    data = request.get_json()
    user_message = data['message']
    current_app.logger.info(f"Received message: {user_message}")
    
    ai_response = agent.process_message(user_message)
    current_app.logger.info(f"AI responded: {ai_response}")

    return jsonify({'status': 'success', 'received_message': user_message, 'ai_response': ai_response})

@app.route('/send_persona_message', methods=['POST'])
def send_persona_message():
    data = request.get_json()
    persona_message = data['message']
    current_app.logger.info(f"Received persona message: {persona_message}")
    
    # process_persona'dan dönen mesajı doğrudan döndür
    persona_status_message = agent.process_persona(persona_message) 
    current_app.logger.info(f"Persona ayarı durumu: {persona_status_message}")

    return jsonify({'status': 'success', 'received_message': persona_message, 'persona_status': persona_status_message})