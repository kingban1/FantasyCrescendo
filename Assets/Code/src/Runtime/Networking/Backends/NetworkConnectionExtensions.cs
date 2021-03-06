﻿using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Networking = HouraiTeahouse.FantasyCrescendo.Networking.NetworkConnection;

namespace HouraiTeahouse.FantasyCrescendo.Networking {


public static class NetworkConnectionExtensions {

  public static void Send(this NetworkConnection connection, byte header, 
                          INetworkSerializable message, NetworkReliablity reliablity = NetworkReliablity.Reliable) {
    var writer = new Serializer();
    writer.Write(header);
    message.Serialize(writer);
    connection.SendBytes(writer.AsArray(), writer.Position, reliablity);
    (message as IDisposable)?.Dispose();
  }

  public static void SendToAll<T>(this IEnumerable<NetworkConnection> connections, byte header,
                                  T message, NetworkReliablity reliablity = NetworkReliablity.Reliable) 
                                  where T : INetworkSerializable {
    var writer = new Serializer();
    writer.Write(header);
    message.Serialize(writer);
    var bufferSize = writer.Position;
    var buffer = writer.AsArray();
    foreach (var connection in connections) {
      connection.SendBytes(buffer, bufferSize, reliablity);
    }
    (message as IDisposable)?.Dispose();
  }

  public static void Send(this NetworkConnection connection, byte header, 
                          MessageBase message, NetworkReliablity reliablity = NetworkReliablity.Reliable) {
    var writer = new NetworkWriter();
    writer.Write(header);
    message.Serialize(writer);
    connection.SendBytes(writer.AsArray(), writer.Position, reliablity);
    (message as IDisposable)?.Dispose();
  }

  public static void SendToAll(this IEnumerable<NetworkConnection> connections, byte header,
                               MessageBase message, NetworkReliablity reliablity = NetworkReliablity.Reliable) {
    var writer = new NetworkWriter();
    writer.Write(header);
    message.Serialize(writer);
    var bufferSize = writer.Position;
    var buffer = writer.AsArray();
    foreach (var connection in connections) {
      connection.SendBytes(buffer, bufferSize, reliablity);
    }
    (message as IDisposable)?.Dispose();
  }

}

}