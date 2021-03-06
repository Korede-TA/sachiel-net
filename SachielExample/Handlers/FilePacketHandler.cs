﻿using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuf;
using Sachiel;
using Sachiel.Messages;
using Sachiel.Messages.Packets;
using SachielExample.Models;

namespace SachielExample.Handlers
{
    [ProtoContract]
    [SachielHeader(Endpoint = "TreeResponse")]
    public class TreeResponse : Message
    {
        [ProtoMember(1)]
        public FileTree Tree { get; set; }
    }

    internal class FilePacketHandler : PacketHandler
    {
        private PacketCallback _callback;
        private Consumer _consumer;
        private Message _message;
        private Packet _packet;

        public override void HandlePacket(Consumer consumer, Packet packet)
        {
            _consumer = consumer;
            _packet = packet;
            _message = _packet.Message;
            _callback = new PacketCallback {Endpoint = _message.Header.Endpoint };
            switch (_message.Header.Endpoint)
            {
                case "RequestFileTree":
                    RequestFileTree();
                    break;
            }
        }

        private async void RequestFileTree()
        {
            var fileRequest = _message.Deserialize<RequestFileTree>();

            Console.WriteLine(fileRequest.Path);
            Console.WriteLine($"Request for {fileRequest.Path} received");
            var response = new TreeResponse {Tree = new FileTree("D:\\ffmpeg-20170804-44e9783-win64-static", true) };
            _callback.Response = await response.Serialize();
   
            _consumer.Reply(_callback);
        }
    }
}