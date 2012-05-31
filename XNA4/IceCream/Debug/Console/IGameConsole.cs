#if !XNATOUCH
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace IceCream.Debug.Console
{
    
    public interface IGameConsole
    {
        /// <summary>
        /// Event triggered when text is entered into the console
        /// </summary>
        event ConsoleInputHandler TextEntered;

        /// <summary>
        /// Event triggered when a message is logged to the console
        /// </summary>
        event ConsoleLogHandler MessageLogged;

        /// <summary>
        /// Gets or sets the console's text color
        /// </summary>
        Color TextColor { get; set; }

        /// <summary>
        /// Gets or sets the console's background color
        /// </summary>
        Color BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the console's background alpha translucency
        /// </summary>
        float BackgroundAlpha { get; set; }

        /// <summary>
        /// Gets or sets the console's display options
        /// </summary>
        ConsoleDisplayOptions DisplayOptions { get; set; }

        /// <summary>
        /// Gets or sets the timestamp format for message timestamps.
        /// The following symblols are replaced with their corresponding values
        /// when the timestamp for each message is constructed:
        ///     {Hr} - Hour (in 24 hour format)
        ///     {Min} - Minute
        ///     {Sec} - Second
        ///     {Ms} - Millisecond
        /// 
        /// Sample timestamp strings:
        ///     "{Hr}:{Min}:{Sec}"
        ///     "[{Hr}:{Min}:{Sec}:{Ms}]"
        /// </summary>
        string TimestampFormat { get; set; }

        /// <summary>
        /// Gets or sets the prompt used on the input line of the console
        /// </summary>
        string Prompt { get; set; }

        /// <summary>
        /// Gets or sets the number of text lines visible in the
        /// console
        /// </summary>
        int VisibleLineCount { get; set; }

        /// <summary>
        /// Gets or sets the vertical spacing between text lines in
        /// the console
        /// </summary>
        float VerticalLineSpacing { get; set; }

        /// <summary>
        /// Gets or sets the amount by which text in the console will be
        /// right offset from the leftmost side of the screen
        /// </summary>
        float HorizontalPadding { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the console should
        /// automatically scroll to the last line of the message log whenever a 
        /// message is output to the console
        /// </summary>
        bool AutoScrollOnOutput { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the console will output
        /// the warning "Unrecognized Command" if the user inputs a command not
        /// registered with a command handler
        /// </summary>
        bool AlertOnUnrecognizedCommand { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the console
        /// should automatically echo and input it receives from the user
        /// to the console log
        /// </summary>
        bool EchoEnabled { get; set; }

        /// <summary>
        /// Gets or sets the log level of messages echoed automatically
        /// if the EchoEnabled property is set to true
        /// </summary>
        uint EchoLogLevel { get; set; }

        /// <summary>
        /// Gets or sets the console's default log level which will
        /// be assigned to messages logged to the console with no log level
        /// defined
        /// </summary>
        uint DefaultLogLevel { get; set; }

        /// <summary>
        /// Gets or sets the console's log level threshold. If a message's whose
        /// log level is higher than the threshold is logged to the console, it will
        /// be ignored. However, if the value is negative then all messages will be
        /// logged regardless of their log level
        int LogLevelThreshold { get; set; }

        /// <summary>
        /// Gets or sets the console's display level threshold. The console will
        /// only display messages in the log with log levels lower or equal to
        /// the display level threshold. However, if the value is negative,
        /// then all messages will be displayed regardless of their threshold
        /// </summary>
        int DisplayLevelThreshold { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the input cursor
        /// is enabled. If enabled, the user can use the arrow keys to navigate
        /// the edit position within the current input string
        /// </summary>
        bool CursorEnabled { get; set; }

        /// <summary>
        /// Gets or sets the blink interval of the input cursor, in seconds. The cursor
        /// will not blink if this value is set to 0.
        /// </summary>
        float CursorBlinkSpeed { get; set; }

        /// <summary>
        /// Gets or sets the blink interval of the notifier that is displayed when
        /// the console's current line is not the last line of the log, that is: when
        /// the user has used PageUp/Up to scroll upward in the console. The notifier
        /// will not blink if this value is set to 0.
        /// </summary>
        float NotifyBlinkSpeed { get; set; }

        /// <summary>
        /// Gets the console log of messages
        /// </summary>
        ReadOnlyCollection<ConsoleMessage> MessageLog { get; }

        /// <summary>
        /// Gets a value indicating whether or not the console is currently open
        /// </summary>
        bool IsOpen { get; }


        /// <summary>
        /// Binds a ConsoleCommandHandler to the specified command
        /// </summary>
        /// <param name="command">The command to which the command handler will be bound</param>
        /// <param name="handler">The command handler to bind to the specified command</param>
        /// <param name="argumentSeparators">The characters with which the command argument string will
        /// be split into separate arguments</param>
        void BindCommandHandler(string command, ConsoleCommandHandler handler, params char[] argumentSeparators);

        /// <summary>
        /// Unbinds the ConsoleCommandHandler for the specified command
        /// </summary>
        /// <param name="command">The command for which to unbind the current
        /// command handler</param>
        /// <returns>The command handler previously bound to the specified command,
        /// or null if no handler was bound</returns>
        ConsoleCommandHandler UnbindCommandHandler(string command);

        /// <summary>
        /// Displays the console
        /// </summary>
        /// <param name="closeKey">The key that should deactivate the console
        /// when pressed</param>
        void Open(Keys closeKey);

        /// <summary>
        /// Logs a message with the specified log level to the console
        /// </summary>
        /// <param name="message">The message to output to the console</param>
        /// <param name="level">The log level of the message</param>
        void Log(string message, uint level);

        /// <summary>
        /// Logs a message with log level 0 to the console
        /// </summary>
        /// <param name="message">The message to output to the console</param>
        void Log(string message);

        /// <summary>
        /// Sets the custom display color of text for the specified log level
        /// </summary>
        /// <param name="level">The log level for which to set the custom text
        /// display color</param>
        /// <param name="color">The custom color with which to display text for
        /// the specified log level</param>
        void SetLogLevelCustomColor(uint level, Color color);

        /// <summary>
        /// Gets the custom display color of text for the specified log level,
        /// or returns the default text color if a custom color is not
        /// set for the level
        /// </summary>
        /// <param name="level">The log level for which to get the
        /// custom display color</param>
        /// <returns>The custom display color of text for the specified log level, or
        /// the default text color if no custom color is set for the level</returns>
        Color GetLogLevelCustomColor(uint level);

        /// <summary>
        /// Unsets the custom display color of text for the specified log level
        /// </summary>
        /// <param name="level">The log level for which to unset the custom
        /// text color</param>
        void UnsetLogLevelCustomColor(uint level);

        void LoadStuff();
    }

}

#endif