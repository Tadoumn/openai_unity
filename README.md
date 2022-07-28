# openai_unity
A simple implementation of openai API for unity

1: Create an OpenAIAuth scriptable object
2: Add your openai API Key and Organization Key in OpenAIAuth
3: Create an OpenAIConfig and setup your desired config for openai request (Temperature, Max token, temperature)
4: Add OpenAI script as a component on a gameobject
5: Set scriptable OpenAIAuth and OpenAIConfig on the OpenAI component.
5: Call Request function in OpenAI script with your prompt and specific request config (if null it get your default config scriptable)
6: backRequest list is updated with openai answer when it comes.
7: The content follow the data structure of openai anwser
