﻿using System.Collections.Generic;
using SDL2;
using Shift.Input;
using Shift.Input.Util;
using EventType = SDL2.SDL.SDL_EventType;

namespace Shift
{
    class Platform
    {
        private static Game _game;
        private static List<Keys> _keys;

        public static bool IsActive
        {
            get; private set;
        }

        static Platform()
        {
            _keys = new List<Keys>();
            Keyboard.SetKeys(_keys);
        }

        public static void Init(Game game)
        {
            _game = game;
            SDL.SDL_Init(SDL.SDL_INIT_VIDEO);/* | SDL.SDL_INIT_JOYSTICK 
                | SDL.SDL_INIT_GAMECONTROLLER | SDL.SDL_INIT_HAPTIC);**/

            SDL.SDL_DisableScreenSaver();
        }

        public static void ProcessEvents()
        {
            SDL.SDL_Event ev;

            while (SDL.SDL_PollEvent(out ev) == 1)
            {
                if (ev.type == EventType.SDL_QUIT)
                {
                    _game.Running = false;
                    break;
                }
                /*else if (ev.type == EventType.JoyDeviceAdded)
                    Joystick.AddDevice(ev.JoystickDevice.Which);
                else if (ev.type == EventType.ControllerDeviceRemoved)
                    GamePad.RemoveDevice(ev.ControllerDevice.Which);
                else if (ev.type == EventType.JoyDeviceRemoved)
                    Joystick.RemoveDevice(ev.JoystickDevice.Which);*/
                else if (ev.type == EventType.SDL_MOUSEWHEEL)
                {
                    Mouse.ScrollY += ev.wheel.y * 120;
                }
                else if (ev.type == EventType.SDL_KEYDOWN)
                {
                    var key = KeyboardUtil.KeyFromSDLCode((int)ev.key.keysym.sym);
                    if (!_keys.Contains(key))
                        _keys.Add(key);
                    char character = (char)ev.key.keysym.sym;
                    // TODO: Input events
                    /*if (char.IsControl(character))
                        CallTextInput(character, key);*/
                }
                else if (ev.type == EventType.SDL_KEYUP)
                {
                    var key = KeyboardUtil.KeyFromSDLCode((int)ev.key.keysym.sym);
                    _keys.Remove(key);
                }
                else if (ev.type == EventType.SDL_TEXTINPUT)
                {
                    string text;
                    unsafe
                    {
                        text = new string((char*)ev.text.text);
                    }
                    if (text.Length == 0)
                        continue;
                    foreach (var c in text)
                    {
                        var key = KeyboardUtil.KeyFromSDLCode((int)c);
                        // TODO: Text input again
                        /*_view.CallTextInput(c, key);*/
                    }
                }
                else if (ev.type == EventType.SDL_WINDOWEVENT)
                {
                    //if (ev.window.windowEvent == Sdl.Window.EventId.Resized || ev.Window.EventID == Sdl.Window.EventId.SizeChanged)
                    //_view.ClientResize(ev.Window.Data1, ev.Window.Data2);
                    if (ev.window.windowEvent == SDL.SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_GAINED)
                        IsActive = true;
                    else if (ev.window.windowEvent == SDL.SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_LOST)
                        IsActive = false;
                }
            }
        }
    }
}