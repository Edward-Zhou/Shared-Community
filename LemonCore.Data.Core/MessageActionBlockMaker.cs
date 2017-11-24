using LemonCore.Core.Models;
using LemonCore.Core.Services;
using log4net;
using System;
using System.Collections.Generic;
using System.Text;

namespace LemonCore.Core
{
    internal class MessageActionBlockMaker<TMessage>
    {
        protected readonly ILog Logger;
        private Action<TMessage> _action;

        public MessageActionBlockMaker(Action<TMessage> action)
        {
            _action = action;
            Logger = LogService.Default.GetLog("MessageActionBlock");
        }

        private void ActionImpl(MessageWrapper<TMessage> messageWrapper)
        {
            try
            {
                _action(messageWrapper.Message);
            }
            catch (Exception ex)
            {
                if (messageWrapper != null)
                {
                    Logger.Error(
                        string.Format("exception on pipeline {0}, value = {1}", messageWrapper.PipelineId, messageWrapper.Message),
                        ex);
                }
                else
                {
                    Logger.Error("empty message - action", ex);
                }
            }
        }

        public Action<MessageWrapper<TMessage>> Action { get { return ActionImpl; } }
    }
}
