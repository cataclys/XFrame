#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class MessageBoxWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(MessageBox);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 8, 8);
			
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "TextMsg", _g_get_TextMsg);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Background", _g_get_Background);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DialogBox", _g_get_DialogBox);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ButtonOK", _g_get_ButtonOK);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BtnOK", _g_get_BtnOK);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ButtonYesNo", _g_get_ButtonYesNo);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BtnYes", _g_get_BtnYes);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BtnNo", _g_get_BtnNo);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "TextMsg", _s_set_TextMsg);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Background", _s_set_Background);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "DialogBox", _s_set_DialogBox);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ButtonOK", _s_set_ButtonOK);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "BtnOK", _s_set_BtnOK);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ButtonYesNo", _s_set_ButtonYesNo);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "BtnYes", _s_set_BtnYes);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "BtnNo", _s_set_BtnNo);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 2, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "Show", _m_Show_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					MessageBox gen_ret = new MessageBox();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to MessageBox constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Show_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)) 
                {
                    string _msg = LuaAPI.lua_tostring(L, 1);
                    
                    MessageBox.Show( _msg );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    string _msg = LuaAPI.lua_tostring(L, 1);
                    int _fontSize = LuaAPI.xlua_tointeger(L, 2);
                    
                    MessageBox.Show( _msg, _fontSize );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<UnityEngine.Color>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    string _msg = LuaAPI.lua_tostring(L, 1);
                    UnityEngine.Color _color;translator.Get(L, 2, out _color);
                    int _fontSize = LuaAPI.xlua_tointeger(L, 3);
                    
                    MessageBox.Show( _msg, _color, _fontSize );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<UnityEngine.Events.UnityAction>(L, 2)&& translator.Assignable<UnityEngine.Events.UnityAction>(L, 3)) 
                {
                    string _msg = LuaAPI.lua_tostring(L, 1);
                    UnityEngine.Events.UnityAction _yes = translator.GetDelegate<UnityEngine.Events.UnityAction>(L, 2);
                    UnityEngine.Events.UnityAction _no = translator.GetDelegate<UnityEngine.Events.UnityAction>(L, 3);
                    
                    MessageBox.Show( _msg, _yes, _no );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to MessageBox.Show!");
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TextMsg(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MessageBox gen_to_be_invoked = (MessageBox)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TextMsg);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Background(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MessageBox gen_to_be_invoked = (MessageBox)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Background);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DialogBox(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MessageBox gen_to_be_invoked = (MessageBox)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.DialogBox);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ButtonOK(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MessageBox gen_to_be_invoked = (MessageBox)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.ButtonOK);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BtnOK(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MessageBox gen_to_be_invoked = (MessageBox)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.BtnOK);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ButtonYesNo(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MessageBox gen_to_be_invoked = (MessageBox)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.ButtonYesNo);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BtnYes(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MessageBox gen_to_be_invoked = (MessageBox)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.BtnYes);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BtnNo(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MessageBox gen_to_be_invoked = (MessageBox)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.BtnNo);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_TextMsg(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MessageBox gen_to_be_invoked = (MessageBox)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.TextMsg = (UnityEngine.UI.Text)translator.GetObject(L, 2, typeof(UnityEngine.UI.Text));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Background(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MessageBox gen_to_be_invoked = (MessageBox)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Background = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_DialogBox(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MessageBox gen_to_be_invoked = (MessageBox)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.DialogBox = (UnityEngine.UI.Image)translator.GetObject(L, 2, typeof(UnityEngine.UI.Image));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ButtonOK(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MessageBox gen_to_be_invoked = (MessageBox)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ButtonOK = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_BtnOK(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MessageBox gen_to_be_invoked = (MessageBox)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.BtnOK = (UnityEngine.UI.Button)translator.GetObject(L, 2, typeof(UnityEngine.UI.Button));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ButtonYesNo(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MessageBox gen_to_be_invoked = (MessageBox)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ButtonYesNo = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_BtnYes(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MessageBox gen_to_be_invoked = (MessageBox)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.BtnYes = (UnityEngine.UI.Button)translator.GetObject(L, 2, typeof(UnityEngine.UI.Button));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_BtnNo(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MessageBox gen_to_be_invoked = (MessageBox)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.BtnNo = (UnityEngine.UI.Button)translator.GetObject(L, 2, typeof(UnityEngine.UI.Button));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
