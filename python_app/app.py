from flask import Flask
from agent import Agent
from langchain_google_genai import ChatGoogleGenerativeAI
from dotenv import load_dotenv

load_dotenv()

app = Flask(__name__)

# Initialize Agent globally
llm = ChatGoogleGenerativeAI(model="gemini-1.5-flash", temperature=1)
agent = Agent(llm)

# Import routes after agent is initialized
import routes

if __name__ == "__main__":
    # The debug=True flag enables auto-reloading
    app.run(host='0.0.0.0', port=5000, debug=True)