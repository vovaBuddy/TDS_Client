using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.MessageBus
{
	public class MessageQueue
	{
		private Message[] _messages;
		private int _head;
		private int _tail;
		private int _size;
		private int _count;

		private MessageQueue() {}

		public int Count()
		{
			return _count;
		}

		public MessageQueue(int size)
		{
			_size = size;
			_messages = new Message[_size];

			_head = 0;
			_tail = 0;
		}
		
		public void Push(Message msg)
		{
			_messages[_tail] = msg;
			_tail = (_tail + 1) % _size;

			++_count;
		}

		public Message Pop()
		{
			_head %= _size;
			
			if (_head == _tail)
				return null;

			--_count;
			
			return _messages[_head++];
		}
	}
}
