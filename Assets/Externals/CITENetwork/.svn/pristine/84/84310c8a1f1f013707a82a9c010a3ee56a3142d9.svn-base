using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class CITEDiscovery
{
    private int SEEN_PEER_TIMEOUT = 3000;

    private int BROADCAST_PORT = 4260;

    private UdpClient udpClient;

    public delegate void PeerListUpdatedHandler(List<CITEDiscoveryPeer> peers);
    public event PeerListUpdatedHandler onPeerListUpdated;

    private CITEDiscoveryPeer localPeer;

    private bool doBroadcasting;

    private Dictionary<string, CITEDiscoveryPeer> seenPeers;

    private CancellationTokenSource receiveTaskCancellation;
    private CancellationTokenSource sendTaskCancellation;
    private Task sendTask;
    private Task receiveTask;
    private bool keepGoing = true;

    public CITEDiscovery()
    {
        udpClient = new UdpClient();
        udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, BROADCAST_PORT));
        localPeer = new CITEDiscoveryPeer();

        seenPeers = new Dictionary<string, CITEDiscoveryPeer>();

        doBroadcasting = false;

        BinaryFormatter bf = new BinaryFormatter();

        keepGoing = true;

        //Broadcast task
        sendTaskCancellation = new CancellationTokenSource();
        sendTask = Task.Run(() =>
        {
            try
            {
                while (keepGoing)
                {
                    if (doBroadcasting)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            bf.Serialize(ms, localPeer);
                            byte[] data = ms.ToArray();
                            udpClient.Send(data, data.Length, "255.255.255.255", BROADCAST_PORT);
                        }
                    }

                    lock (seenPeers)
                    {
                        //Cleanup our own seenPeers
                        long now = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
                        List<string> toDeleteKeys = new List<string>();
                        foreach (CITEDiscoveryPeer peer in seenPeers.Values)
                        {
                            if (now - peer.LastSeen > SEEN_PEER_TIMEOUT)
                            {
                                toDeleteKeys.Add(peer.Uuid);
                            }
                        }

                        if (toDeleteKeys.Count > 0)
                        {
                            foreach (string key in toDeleteKeys)
                            {
                                seenPeers.Remove(key);
                            }

                            List<CITEDiscoveryPeer> currentPeers = new List<CITEDiscoveryPeer>(seenPeers.Values);
                            if (onPeerListUpdated != null)
                            {
                                onPeerListUpdated(currentPeers);
                            }
                        }
                    }

                    Thread.Sleep(1000);
                }
            }
            catch (Exception e)
            {
                Debug.Log("Exception in sending thread: " + e);
            }
            Debug.Log("Send thread stopped.");
        }, sendTaskCancellation.Token);

        IPEndPoint from = new IPEndPoint(0, 0);
        //Receiver task
        receiveTaskCancellation = new CancellationTokenSource();
        receiveTask = Task.Run(() =>
        {
            try
            {
                while (keepGoing)
                {
                    byte[] data = udpClient.Receive(ref from);
                    using (MemoryStream ms = new MemoryStream(data))
                    {
                        CITEDiscoveryPeer receivedPeer = (CITEDiscoveryPeer)bf.Deserialize(ms);

                        if (receivedPeer.Uuid != localPeer.Uuid)
                        {
                            receivedPeer.NetworkAddress = from.Address.ToString();

                            bool changedSomething = false;

                            lock (seenPeers)
                            {
                                if (seenPeers.ContainsKey(receivedPeer.Uuid))
                                {
                                    //Already has this peer, update it.
                                    changedSomething = seenPeers[receivedPeer.Uuid].updateFrom(receivedPeer);
                                }
                                else
                                {
                                    //New peer add it.
                                    receivedPeer.LastSeen = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
                                    seenPeers[receivedPeer.Uuid] = receivedPeer;
                                    changedSomething = true;
                                }

                                if (changedSomething)
                                {
                                    List<CITEDiscoveryPeer> currentPeers = new List<CITEDiscoveryPeer>(seenPeers.Values);
                                    if (onPeerListUpdated != null)
                                    {
                                        onPeerListUpdated(currentPeers);
                                    }
                                }
                            }
                        }
                        else
                        {
                            //We saw our own peer, skip it
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log("Error in receiver thread: " + e);
            }
            Debug.Log("Receive thread stopped.");
        }, receiveTaskCancellation.Token);
    }

    public void Destroy()
    {
        keepGoing = false;
        udpClient.Close();
        sendTaskCancellation.Cancel();
        sendTask.Wait(2500);
        receiveTaskCancellation.Cancel();
        receiveTask.Wait(2500);
        Debug.Log("Tasks cancelled!");
    }

    public void resetSeenPeers()
    {
        lock (seenPeers)
        {
            seenPeers.Clear();
        }
    }

    public CITEDiscoveryPeer getLocalPeer()
    {
        return localPeer;
    }

    public void startBroadcasting()
    {
        doBroadcasting = true;
    }

    public void stopBroadcasting()
    {
        doBroadcasting = false;
    }

    [Serializable]
    public class CITEDiscoveryPeer : IEquatable<CITEDiscoveryPeer>
    {
        private string uuid;
        private string deviceName;
        private bool isServer;
        private string networkAddress;
        private string roomId;
        private long lastSeen;

        public CITEDiscoveryPeer()
        {
            Uuid = null;
            DeviceName = null;
            IsServer = false;
            NetworkAddress = null;
            RoomId = null;
            LastSeen = 0;
        }

        public bool updateFrom(CITEDiscoveryPeer updatePeer)
        {
            bool changedSomething = false;
            if (!this.Uuid.Equals(updatePeer.Uuid))
            {
                this.Uuid = updatePeer.Uuid;
                changedSomething = true;
            }

            if (!this.DeviceName.Equals(updatePeer.DeviceName))
            {
                this.DeviceName = updatePeer.DeviceName;
                changedSomething = true;
            }

            if (this.IsServer != updatePeer.IsServer)
            {
                this.IsServer = updatePeer.IsServer;
                changedSomething = true;
            }

            if (!this.NetworkAddress.Equals(updatePeer.NetworkAddress))
            {
                this.NetworkAddress = updatePeer.NetworkAddress;
                changedSomething = true;
            }

            if (!this.RoomId.Equals(updatePeer.RoomId))
            {
                this.RoomId = updatePeer.RoomId;
                changedSomething = true;
            }

            LastSeen = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();

            return changedSomething;
        }

        public override string ToString()
        {
            return "[" + roomId + " - " + uuid + "] - " + deviceName + " @ " + networkAddress + " (Server: " + isServer + ")";
        }

        public bool Equals(CITEDiscoveryPeer other)
        {
            return other != null && this.Uuid.Equals(other.Uuid);
        }

        public string RoomId { get => roomId; set => roomId = value; }
        public string NetworkAddress { get => networkAddress; set => networkAddress = value; }
        public bool IsServer { get => isServer; set => isServer = value; }
        public string DeviceName { get => deviceName; set => deviceName = value; }
        public string Uuid { get => uuid; set => uuid = value; }
        public long LastSeen { get => lastSeen; set => lastSeen = value; }
    }
}
