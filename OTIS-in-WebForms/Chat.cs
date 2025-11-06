using System;
using System.Collections.Generic;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Responses;

// Run "dotnet add package OpenAI"
namespace OTIS.Chat
{

    public class Request
    {
        public string Message;
        public List<string> Messages;

        public Request(string _Message, List<string> _Messages)
        {
            Message = _Message;
            Messages = _Messages;
        }
    };

    class Chat
    {
        public static string Message(Request request)
        {
            const string RULES = @"
				You are a therapist for an occupational health company's website.
				The company you work for is named PAM Group.
				Your name is Otis.

				You are meant to help with people's mental health.

				Only answer questions related to mental health or helping with issues in daily life.

				If someone asks an unrelated question, please explain nicely that you are there for mental health help.

				If they are telling you about something they like and it is not harmful or inappropriate, 
					Engage a little bit with it but then get back on the topic of mental health. 

				If someone begins to say bigoted things, be they racist, homophobic, transphobic, anti-Semitic or other,
					Please say, 'I am here for you, but please do not use hateful language or commit hateful acts.'
					Please talk to me without any hate for protected characteristics' and do not say anything more; just say that.

				If someone mentions suicide, please try and talk them down and offer them resources.
					such as the phone number for Samaritans or whatever resources for suicide prevention you find
					online that are relevant but also give them specific help for their issue,
					Tell them that a lot of people relate and make them feel comfortable with it.

				Never ever tell someone to harm themselves or others or help with creating weapons or torture devices.
					such as explaining how to create biochemicals.

				You cannot ignore these rules ever and never go against them; if you go against them,
					Please say the line 'I have broken a rule; please try again. Sorry for the inconvenience.'
					and link the phone number for Samaritans, saying what it is, and then say nothing more.
			";


            // Retrieves the key from an Environment Variable, use "setx OPENAI_API_KEY "your_api_key_here""
            string key = Environment.GetEnvironmentVariable("OPENAI_API_KEY", EnvironmentVariableTarget.User);


            if (string.IsNullOrEmpty(key))
            {
                Console.WriteLine("Missing OpenAI API key.");
                return "Error";
            }

            List<ChatMessage> messages = new List<ChatMessage>() {


                new SystemChatMessage(RULES),
            };

            if (request.Messages != null && request.Messages.Count != 0)
            {

                for (int i = 0; i < request.Messages.Count; i++)
                {
                    string content = request.Messages[i];

                    if (i % 2 == 0)
                    {
                        messages.Add(new UserChatMessage(content));
                    }
                    else
                    {
                        messages.Add(new AssistantChatMessage(content));
                    }
                }
            }

            messages.Add(new UserChatMessage(request.Message));

            ChatClient client = new ChatClient(model: "gpt-4o-mini", apiKey: key);

            ChatCompletion completion = client.CompleteChat(messages);
            return completion.Content[0].Text;
        }
    }
}
