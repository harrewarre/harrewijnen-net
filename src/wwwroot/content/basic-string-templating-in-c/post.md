# Basic string templating in C#

Every solution I've worked on required some form of communication via e-mail or other form of text to the outside world, reporting, notifications or something else. There are a bunch of libraries out there that can make your life easy if you need complex templates. But this is not about complex, this is supporting the basics without including all sorts of unnecessary stuff.

What do we need to make this happen? The first thing is a template. A template is nothing more than a piece of plain text (or HTML if you want) in which we want to replace parts to create a complete message. Lets use something very simple, a single line, one tag.

    Reminder: %reminderText%
	
The `%reminderText%` is the part we are most interested in, because that is what we want to set ourselves.

To bring the template into the world of C#, we create a Template class.

    public class Template
	{
		private string TemplateText;
	
		public Template(string template)
		{
			TemplateText = template;
		}
		
		public void Fill(Dictionary<string,string> tags)
		{
			foreach(var t in tags)
			{
				TemplateText = TemplateText.Replace(string.Format("%{0}%", t.Key), t.Value);
			}
		}
		
		public string GetText()
		{
			return TemplateText;
		}
	}
	
The constructor takes in the untouched template. The `Fill` method will take care of replacing the tags inside the template with actual text and the `GetText` method will give us the resulting filled template.
	
We also want something that represents the message we want to send. Lets use an interface for that, we want to create different kinds of messages that should all behave the same.

	public interface IMessage
	{
		Dictionary<string,string> GetParameters();
	
		string TemplateKey { get; }
	}
	
The `GetParameters` method will return the tags we want to use in our message and the `TemplateKey` stores the name of the template to use for the message (more on that later).

The third thing we need is some code that will consume both of these and use it to create an actual message.

	public class Messenger
	{
		// Code to get the thing that loads the template from somewhere by its key (database, file, etc)
	
		public void Send(IMessage message)
		{
			var templateText = Something.GetTemplate(message.TemplateKey); // The Something is out of scope here. Where the template comes from is up to you.
			var template = new Template(templateText);
			
			template.Fill(message.GetParameters());
			
			var finalText = template.GetText();
			
			// Code to send the message here.
		}
	}
	
All the parts are there except for one, we don't have a message to send. We only have the `IMessage` interface. Lets make an implementation that works with the template we have.

	public class SingleLineMessage : IMessage
	{
		private readonly Dictionary<string, string> Tags;
	
		public SingleLineMessage(string reminderText)
		{
			Tags = new Dictionary<string,string>();
			Tags.Add("reminderText", reminderText);
		}
		
		public Dictionary<string,string> GetParameters()
		{
			return Tags;
		}
		
		public string TemplateKey
		{
			get { return "SingleLineMessage"; }
		}
	}
	
We've implemented the `IMessage` interface and the constructor takes in the one value for the single tag we have in the template. The key is there and can be used to locate a template by this key in another system (which is outside of the scope of this post). Time to wire everything together!

	var message = new SingleLineMessage("Buy bacon!");
	var messenger = new Messenger();
	
	messenger.Send(message);
	
The `finalText` variable in the Messenger object now reads `Reminder: Buy bacon!`.

With some simple constructs we now have a basic string templating system that ensures we supply all the required values to a given template. The message and template are linked together by their key and the constructor of the message enforces that every tag in the template gets a value. You now have a way of defining all sorts of messages such as e-mail, documents and reports based on templates. Do keep in mind that this is all very rudimentary and lacks things such as creating lists of values for a single tag. Anything more complex and you should look for libraries that handle the work for you. 