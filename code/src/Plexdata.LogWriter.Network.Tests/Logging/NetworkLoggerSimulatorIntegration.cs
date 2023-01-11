/*
 * MIT License
 * 
 * Copyright (c) 2022 plexdata.de
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using NUnit.Framework;
using NUnit.Framework.Internal;
using Plexdata.LogWriter.Abstraction;
using Plexdata.LogWriter.Definitions;
using Plexdata.LogWriter.Logging;
using Plexdata.LogWriter.Settings;
using Plexdata.Utilities.Testing;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Plexdata.LogWriter.Network.Tests.Logging
{
    /* ----------------------------------------------------------------------
     * 
     * ATTENTION!
     * 
     * Run these tests only together with the 'Plexdata.Graylog.Simulator'.
     * 
     * ---------------------------------------------------------------------- */

    [TestFixture]
    [ExcludeFromCodeCoverage]
    [Category(TestType.CompatibilityTest)]
    [TestOf(nameof(NetworkLoggerBase))]
    public class NetworkLoggerSimulatorIntegration
    {
        #region UDP

        [Explicit]
        [TestCase("127.0.0.1", 42011, Address.IPv4, "111")]
        [TestCase("localhost", 42011, Address.IPv4, "222")]
        [TestCase("::1", 42012, Address.IPv6, "333")]
        [TestCase("localhost", 42012, Address.IPv6, "444")]
        public void MessageUdp_MinimalNoContextNoCompressionNoChunking_MessageSent(String host, Int32 port, Address address, String suffix)
        {
            INetworkLoggerSettings settings = new NetworkLoggerSettings()
            {
                LogLevel = LogLevel.Trace,
                LogType = LogType.Gelf,
                Host = host,
                Port = port,
                Address = address,
                Protocol = Protocol.Udp,
                ShowKey = false,
                ShowTime = false,
                Compressed = false
            };

            using (INetworkLogger instance = this.CreateDefault(settings))
            {
                instance.Write(LogLevel.Trace, this.CreateMessage(30, "UDP", "TRC", suffix));
                instance.Write(LogLevel.Debug, this.CreateMessage(30, "UDP", "DBG", suffix));
                instance.Write(LogLevel.Verbose, this.CreateMessage(30, "UDP", "VBS", suffix));
                instance.Write(LogLevel.Message, this.CreateMessage(30, "UDP", "MSG", suffix));
                instance.Write(LogLevel.Warning, this.CreateMessage(30, "UDP", "WRN", suffix));
                instance.Write(LogLevel.Error, this.CreateMessage(30, "UDP", "ERR", suffix));
                instance.Write(LogLevel.Fatal, this.CreateMessage(30, "UDP", "FAT", suffix));
                instance.Write(LogLevel.Critical, this.CreateMessage(30, "UDP", "CRT", suffix));
                instance.Write(LogLevel.Disaster, this.CreateMessage(30, "UDP", "DST", suffix));
            }
        }

        [Explicit]
        [TestCase("127.0.0.1", 42011, Address.IPv4, "111")]
        [TestCase("localhost", 42011, Address.IPv4, "222")]
        [TestCase("::1", 42012, Address.IPv6, "333")]
        [TestCase("localhost", 42012, Address.IPv6, "444")]
        public void MessageUdp_StandardNoCompressionNoChunking_MessageSent(String host, Int32 port, Address address, String suffix)
        {
            INetworkLoggerSettings settings = new NetworkLoggerSettings()
            {
                LogLevel = LogLevel.Trace,
                LogType = LogType.Gelf,
                Host = host,
                Port = port,
                Address = address,
                Protocol = Protocol.Udp,
                ShowKey = true,
                ShowTime = true,
                Compressed = false
            };

            Exception exception = this.CreateException();
            (String Label, Object Value)[] details = this.CreateDetails();

            using (INetworkLogger instance = this.CreateDefault(settings))
            {
                instance.Write("scope", LogLevel.Trace, this.CreateMessage(30, "UDP", "TRC", suffix), exception, details);
                instance.Write("scope", LogLevel.Debug, this.CreateMessage(30, "UDP", "DBG", suffix), exception, details);
                instance.Write("scope", LogLevel.Verbose, this.CreateMessage(30, "UDP", "VBS", suffix), exception, details);
                instance.Write("scope", LogLevel.Message, this.CreateMessage(30, "UDP", "MSG", suffix), exception, details);
                instance.Write("scope", LogLevel.Warning, this.CreateMessage(30, "UDP", "WRN", suffix), exception, details);
                instance.Write("scope", LogLevel.Error, this.CreateMessage(30, "UDP", "ERR", suffix), exception, details);
                instance.Write("scope", LogLevel.Fatal, this.CreateMessage(30, "UDP", "FAT", suffix), exception, details);
                instance.Write("scope", LogLevel.Critical, this.CreateMessage(30, "UDP", "CRT", suffix), exception, details);
                instance.Write("scope", LogLevel.Disaster, this.CreateMessage(30, "UDP", "DST", suffix), exception, details);
            }
        }

        [Explicit]
        [TestCase("127.0.0.1", 42011, Address.IPv4, "111")]
        [TestCase("localhost", 42011, Address.IPv4, "222")]
        [TestCase("::1", 42012, Address.IPv6, "333")]
        [TestCase("localhost", 42012, Address.IPv6, "444")]
        public void MessageUdp_StandardNoCompressionButChunking_MessageSent(String host, Int32 port, Address address, String suffix)
        {
            INetworkLoggerSettings settings = new NetworkLoggerSettings()
            {
                LogLevel = LogLevel.Trace,
                LogType = LogType.Gelf,
                Host = host,
                Port = port,
                Address = address,
                Protocol = Protocol.Udp,
                ShowKey = true,
                ShowTime = true,
                Compressed = false,
                Maximum = 250
            };

            Exception exception = this.CreateException();
            (String Label, Object Value)[] details = this.CreateDetails();

            using (INetworkLogger instance = this.CreateDefault(settings))
            {
                instance.Write("scope", LogLevel.Trace, this.CreateMessage(30, "UDP", "TRC", suffix), exception, details);
                instance.Write("scope", LogLevel.Debug, this.CreateMessage(30, "UDP", "DBG", suffix), exception, details);
                instance.Write("scope", LogLevel.Verbose, this.CreateMessage(30, "UDP", "VBS", suffix), exception, details);
                instance.Write("scope", LogLevel.Message, this.CreateMessage(30, "UDP", "MSG", suffix), exception, details);
                instance.Write("scope", LogLevel.Warning, this.CreateMessage(30, "UDP", "WRN", suffix), exception, details);
                instance.Write("scope", LogLevel.Error, this.CreateMessage(30, "UDP", "ERR", suffix), exception, details);
                instance.Write("scope", LogLevel.Fatal, this.CreateMessage(30, "UDP", "FAT", suffix), exception, details);
                instance.Write("scope", LogLevel.Critical, this.CreateMessage(30, "UDP", "CRT", suffix), exception, details);
                instance.Write("scope", LogLevel.Disaster, this.CreateMessage(30, "UDP", "DST", suffix), exception, details);
            }
        }

        [Explicit]
        [TestCase("127.0.0.1", 42011, Address.IPv4, "111")]
        [TestCase("localhost", 42011, Address.IPv4, "222")]
        [TestCase("::1", 42012, Address.IPv6, "333")]
        [TestCase("localhost", 42012, Address.IPv6, "444")]
        public void MessageUdp_StandardWithCompressionNoChunking_MessageSent(String host, Int32 port, Address address, String suffix)
        {
            INetworkLoggerSettings settings = new NetworkLoggerSettings()
            {
                LogLevel = LogLevel.Trace,
                LogType = LogType.Gelf,
                Host = host,
                Port = port,
                Address = address,
                Protocol = Protocol.Udp,
                ShowKey = true,
                ShowTime = true,
                Compressed = true,
                Threshold = 150
            };

            Exception exception = this.CreateException();
            (String Label, Object Value)[] details = this.CreateDetails();

            using (INetworkLogger instance = this.CreateDefault(settings))
            {
                instance.Write("scope", LogLevel.Trace, this.CreateMessage(30, "UDP", "TRC", suffix), exception, details);
                instance.Write("scope", LogLevel.Debug, this.CreateMessage(30, "UDP", "DBG", suffix), exception, details);
                instance.Write("scope", LogLevel.Verbose, this.CreateMessage(30, "UDP", "VBS", suffix), exception, details);
                instance.Write("scope", LogLevel.Message, this.CreateMessage(30, "UDP", "MSG", suffix), exception, details);
                instance.Write("scope", LogLevel.Warning, this.CreateMessage(30, "UDP", "WRN", suffix), exception, details);
                instance.Write("scope", LogLevel.Error, this.CreateMessage(30, "UDP", "ERR", suffix), exception, details);
                instance.Write("scope", LogLevel.Fatal, this.CreateMessage(30, "UDP", "FAT", suffix), exception, details);
                instance.Write("scope", LogLevel.Critical, this.CreateMessage(30, "UDP", "CRT", suffix), exception, details);
                instance.Write("scope", LogLevel.Disaster, this.CreateMessage(30, "UDP", "DST", suffix), exception, details);
            }
        }

        [Explicit]
        [TestCase("127.0.0.1", 42011, Address.IPv4, "111")]
        [TestCase("localhost", 42011, Address.IPv4, "222")]
        [TestCase("::1", 42012, Address.IPv6, "333")]
        [TestCase("localhost", 42012, Address.IPv6, "444")]
        public void MessageUdp_StandardWithCompressionAndChunking_MessageSent(String host, Int32 port, Address address, String suffix)
        {
            INetworkLoggerSettings settings = new NetworkLoggerSettings()
            {
                LogLevel = LogLevel.Trace,
                LogType = LogType.Gelf,
                Host = host,
                Port = port,
                Address = address,
                Protocol = Protocol.Udp,
                ShowKey = true,
                ShowTime = true,
                Compressed = true,
                Threshold = 150,
                Maximum = 250
            };

            Exception exception = this.CreateException();
            (String Label, Object Value)[] details = this.CreateDetails();

            using (INetworkLogger instance = this.CreateDefault(settings))
            {
                instance.Write("scope", LogLevel.Trace, this.CreateMessage(30, "UDP", "TRC", suffix), exception, details);
                instance.Write("scope", LogLevel.Debug, this.CreateMessage(30, "UDP", "DBG", suffix), exception, details);
                instance.Write("scope", LogLevel.Verbose, this.CreateMessage(30, "UDP", "VBS", suffix), exception, details);
                instance.Write("scope", LogLevel.Message, this.CreateMessage(30, "UDP", "MSG", suffix), exception, details);
                instance.Write("scope", LogLevel.Warning, this.CreateMessage(30, "UDP", "WRN", suffix), exception, details);
                instance.Write("scope", LogLevel.Error, this.CreateMessage(30, "UDP", "ERR", suffix), exception, details);
                instance.Write("scope", LogLevel.Fatal, this.CreateMessage(30, "UDP", "FAT", suffix), exception, details);
                instance.Write("scope", LogLevel.Critical, this.CreateMessage(30, "UDP", "CRT", suffix), exception, details);
                instance.Write("scope", LogLevel.Disaster, this.CreateMessage(30, "UDP", "DST", suffix), exception, details);
            }
        }

        [Explicit]
        [TestCase("127.0.0.1", 42011, Address.IPv4, "111")]
        [TestCase("localhost", 42011, Address.IPv4, "222")]
        [TestCase("::1", 42012, Address.IPv6, "333")]
        [TestCase("localhost", 42012, Address.IPv6, "444")]
        public void MessageUdp_ContextNoCompressionNoChunking_MessageSent(String host, Int32 port, Address address, String suffix)
        {
            INetworkLoggerSettings settings = new NetworkLoggerSettings()
            {
                LogLevel = LogLevel.Trace,
                LogType = LogType.Gelf,
                Host = host,
                Port = port,
                Address = address,
                Protocol = Protocol.Udp,
                ShowKey = true,
                ShowTime = true,
                Compressed = false
            };

            Exception exception = this.CreateException();
            (String Label, Object Value)[] details = this.CreateDetails();

            using (INetworkLogger<ContextTestClass> instance = this.CreateContext(settings))
            {
                instance.Write("scope", LogLevel.Trace, this.CreateMessage(30, "UDP", "TRC", suffix), exception, details);
                instance.Write("scope", LogLevel.Debug, this.CreateMessage(30, "UDP", "DBG", suffix), exception, details);
                instance.Write("scope", LogLevel.Verbose, this.CreateMessage(30, "UDP", "VBS", suffix), exception, details);
                instance.Write("scope", LogLevel.Message, this.CreateMessage(30, "UDP", "MSG", suffix), exception, details);
                instance.Write("scope", LogLevel.Warning, this.CreateMessage(30, "UDP", "WRN", suffix), exception, details);
                instance.Write("scope", LogLevel.Error, this.CreateMessage(30, "UDP", "ERR", suffix), exception, details);
                instance.Write("scope", LogLevel.Fatal, this.CreateMessage(30, "UDP", "FAT", suffix), exception, details);
                instance.Write("scope", LogLevel.Critical, this.CreateMessage(30, "UDP", "CRT", suffix), exception, details);
                instance.Write("scope", LogLevel.Disaster, this.CreateMessage(30, "UDP", "DST", suffix), exception, details);
            }
        }

        [Explicit]
        [TestCase("127.0.0.1", 42011, Address.IPv4, "111")]
        [TestCase("localhost", 42011, Address.IPv4, "222")]
        [TestCase("::1", 42012, Address.IPv6, "333")]
        [TestCase("localhost", 42012, Address.IPv6, "444")]
        public void MessageUdp_ContextNoCompressionButChunking_MessageSent(String host, Int32 port, Address address, String suffix)
        {
            INetworkLoggerSettings settings = new NetworkLoggerSettings()
            {
                LogLevel = LogLevel.Trace,
                LogType = LogType.Gelf,
                Host = host,
                Port = port,
                Address = address,
                Protocol = Protocol.Udp,
                ShowKey = true,
                ShowTime = true,
                Compressed = false,
                Maximum = 250
            };

            Exception exception = this.CreateException();
            (String Label, Object Value)[] details = this.CreateDetails();

            using (INetworkLogger<ContextTestClass> instance = this.CreateContext(settings))
            {
                instance.Write("scope", LogLevel.Trace, this.CreateMessage(30, "UDP", "TRC", suffix), exception, details);
                instance.Write("scope", LogLevel.Debug, this.CreateMessage(30, "UDP", "DBG", suffix), exception, details);
                instance.Write("scope", LogLevel.Verbose, this.CreateMessage(30, "UDP", "VBS", suffix), exception, details);
                instance.Write("scope", LogLevel.Message, this.CreateMessage(30, "UDP", "MSG", suffix), exception, details);
                instance.Write("scope", LogLevel.Warning, this.CreateMessage(30, "UDP", "WRN", suffix), exception, details);
                instance.Write("scope", LogLevel.Error, this.CreateMessage(30, "UDP", "ERR", suffix), exception, details);
                instance.Write("scope", LogLevel.Fatal, this.CreateMessage(30, "UDP", "FAT", suffix), exception, details);
                instance.Write("scope", LogLevel.Critical, this.CreateMessage(30, "UDP", "CRT", suffix), exception, details);
                instance.Write("scope", LogLevel.Disaster, this.CreateMessage(30, "UDP", "DST", suffix), exception, details);
            }
        }

        [Explicit]
        [TestCase("127.0.0.1", 42011, Address.IPv4, "111")]
        [TestCase("localhost", 42011, Address.IPv4, "222")]
        [TestCase("::1", 42012, Address.IPv6, "333")]
        [TestCase("localhost", 42012, Address.IPv6, "444")]
        public void MessageUdp_ContextWithCompressionNoChunking_MessageSent(String host, Int32 port, Address address, String suffix)
        {
            INetworkLoggerSettings settings = new NetworkLoggerSettings()
            {
                LogLevel = LogLevel.Trace,
                LogType = LogType.Gelf,
                Host = host,
                Port = port,
                Address = address,
                Protocol = Protocol.Udp,
                ShowKey = true,
                ShowTime = true,
                Compressed = true,
                Threshold = 150
            };

            Exception exception = this.CreateException();
            (String Label, Object Value)[] details = this.CreateDetails();

            using (INetworkLogger<ContextTestClass> instance = this.CreateContext(settings))
            {
                instance.Write("scope", LogLevel.Trace, this.CreateMessage(30, "UDP", "TRC", suffix), exception, details);
                instance.Write("scope", LogLevel.Debug, this.CreateMessage(30, "UDP", "DBG", suffix), exception, details);
                instance.Write("scope", LogLevel.Verbose, this.CreateMessage(30, "UDP", "VBS", suffix), exception, details);
                instance.Write("scope", LogLevel.Message, this.CreateMessage(30, "UDP", "MSG", suffix), exception, details);
                instance.Write("scope", LogLevel.Warning, this.CreateMessage(30, "UDP", "WRN", suffix), exception, details);
                instance.Write("scope", LogLevel.Error, this.CreateMessage(30, "UDP", "ERR", suffix), exception, details);
                instance.Write("scope", LogLevel.Fatal, this.CreateMessage(30, "UDP", "FAT", suffix), exception, details);
                instance.Write("scope", LogLevel.Critical, this.CreateMessage(30, "UDP", "CRT", suffix), exception, details);
                instance.Write("scope", LogLevel.Disaster, this.CreateMessage(30, "UDP", "DST", suffix), exception, details);
            }
        }

        [Explicit]
        [TestCase("127.0.0.1", 42011, Address.IPv4, "111")]
        [TestCase("localhost", 42011, Address.IPv4, "222")]
        [TestCase("::1", 42012, Address.IPv6, "333")]
        [TestCase("localhost", 42012, Address.IPv6, "444")]
        public void MessageUdp_ContextWithCompressionAndChunking_MessageSent(String host, Int32 port, Address address, String suffix)
        {
            INetworkLoggerSettings settings = new NetworkLoggerSettings()
            {
                LogLevel = LogLevel.Trace,
                LogType = LogType.Gelf,
                Host = host,
                Port = port,
                Address = address,
                Protocol = Protocol.Udp,
                ShowKey = true,
                ShowTime = true,
                Compressed = true,
                Threshold = 150,
                Maximum = 250
            };

            Exception exception = this.CreateException();
            (String Label, Object Value)[] details = this.CreateDetails();

            using (INetworkLogger<ContextTestClass> instance = this.CreateContext(settings))
            {
                instance.Write("scope", LogLevel.Trace, this.CreateMessage(30, "UDP", "TRC", suffix), exception, details);
                instance.Write("scope", LogLevel.Debug, this.CreateMessage(30, "UDP", "DBG", suffix), exception, details);
                instance.Write("scope", LogLevel.Verbose, this.CreateMessage(30, "UDP", "VBS", suffix), exception, details);
                instance.Write("scope", LogLevel.Message, this.CreateMessage(30, "UDP", "MSG", suffix), exception, details);
                instance.Write("scope", LogLevel.Warning, this.CreateMessage(30, "UDP", "WRN", suffix), exception, details);
                instance.Write("scope", LogLevel.Error, this.CreateMessage(30, "UDP", "ERR", suffix), exception, details);
                instance.Write("scope", LogLevel.Fatal, this.CreateMessage(30, "UDP", "FAT", suffix), exception, details);
                instance.Write("scope", LogLevel.Critical, this.CreateMessage(30, "UDP", "CRT", suffix), exception, details);
                instance.Write("scope", LogLevel.Disaster, this.CreateMessage(30, "UDP", "DST", suffix), exception, details);
            }
        }

        #endregion

        #region TCP

        [Explicit]
        [TestCase("127.0.0.1", 42021, Address.IPv4, "111")]
        [TestCase("localhost", 42021, Address.IPv4, "222")]
        [TestCase("::1", 42022, Address.IPv6, "333")]
        [TestCase("localhost", 42022, Address.IPv6, "444")]
        public void MessageTcp_MinimalNoTermination_MessageSent(String host, Int32 port, Address address, String suffix)
        {
            INetworkLoggerSettings settings = new NetworkLoggerSettings()
            {
                LogLevel = LogLevel.Trace,
                LogType = LogType.Gelf,
                Host = host,
                Port = port,
                Address = address,
                Protocol = Protocol.Tcp,
                ShowKey = false,
                ShowTime = false,
                Termination = false
            };

            Exception exception = this.CreateException();
            (String Label, Object Value)[] details = this.CreateDetails();

            using (INetworkLogger instance = this.CreateDefault(settings))
            {
                instance.Write(LogLevel.Trace, this.CreateMessage(30, "TCP", "TRC", suffix));
                instance.Write(LogLevel.Debug, this.CreateMessage(30, "TCP", "DBG", suffix));
                instance.Write(LogLevel.Verbose, this.CreateMessage(30, "TCP", "VBS", suffix));
                instance.Write(LogLevel.Message, this.CreateMessage(30, "TCP", "MSG", suffix));
                instance.Write(LogLevel.Warning, this.CreateMessage(30, "TCP", "WRN", suffix));
                instance.Write(LogLevel.Error, this.CreateMessage(30, "TCP", "ERR", suffix));
                instance.Write(LogLevel.Fatal, this.CreateMessage(30, "TCP", "FAT", suffix));
                instance.Write(LogLevel.Critical, this.CreateMessage(30, "TCP", "CRT", suffix));
                instance.Write(LogLevel.Disaster, this.CreateMessage(30, "TCP", "DST", suffix));
            }
        }

        [Explicit]
        [TestCase("127.0.0.1", 42021, Address.IPv4, "111")]
        [TestCase("localhost", 42021, Address.IPv4, "222")]
        [TestCase("::1", 42022, Address.IPv6, "333")]
        [TestCase("localhost", 42022, Address.IPv6, "444")]
        public void MessageTcp_MinimalWithTermination_MessageSent(String host, Int32 port, Address address, String suffix)
        {
            INetworkLoggerSettings settings = new NetworkLoggerSettings()
            {
                LogLevel = LogLevel.Trace,
                LogType = LogType.Gelf,
                Host = host,
                Port = port,
                Address = address,
                Protocol = Protocol.Tcp,
                ShowKey = false,
                ShowTime = false,
                Termination = true
            };

            using (INetworkLogger instance = this.CreateDefault(settings))
            {
                instance.Write(LogLevel.Trace, this.CreateMessage(30, "TCP", "TRC", suffix));
                instance.Write(LogLevel.Debug, this.CreateMessage(30, "TCP", "DBG", suffix));
                instance.Write(LogLevel.Verbose, this.CreateMessage(30, "TCP", "VBS", suffix));
                instance.Write(LogLevel.Message, this.CreateMessage(30, "TCP", "MSG", suffix));
                instance.Write(LogLevel.Warning, this.CreateMessage(30, "TCP", "WRN", suffix));
                instance.Write(LogLevel.Error, this.CreateMessage(30, "TCP", "ERR", suffix));
                instance.Write(LogLevel.Fatal, this.CreateMessage(30, "TCP", "FAT", suffix));
                instance.Write(LogLevel.Critical, this.CreateMessage(30, "TCP", "CRT", suffix));
                instance.Write(LogLevel.Disaster, this.CreateMessage(30, "TCP", "DST", suffix));
            }
        }

        [Explicit]
        [TestCase("127.0.0.1", 42021, Address.IPv4, "111")]
        [TestCase("localhost", 42021, Address.IPv4, "222")]
        [TestCase("::1", 42022, Address.IPv6, "333")]
        [TestCase("localhost", 42022, Address.IPv6, "444")]
        public void MessageTcp_StandardNoTermination_MessageSent(String host, Int32 port, Address address, String suffix)
        {
            INetworkLoggerSettings settings = new NetworkLoggerSettings()
            {
                LogLevel = LogLevel.Trace,
                LogType = LogType.Gelf,
                Host = host,
                Port = port,
                Address = address,
                Protocol = Protocol.Tcp,
                ShowKey = false,
                ShowTime = false,
                Termination = false
            };

            Exception exception = this.CreateException();
            (String Label, Object Value)[] details = this.CreateDetails();

            using (INetworkLogger instance = this.CreateDefault(settings))
            {
                instance.Write("scope", LogLevel.Trace, this.CreateMessage(30, "TCP", "TRC", suffix), exception, details);
                instance.Write("scope", LogLevel.Debug, this.CreateMessage(30, "TCP", "DBG", suffix), exception, details);
                instance.Write("scope", LogLevel.Verbose, this.CreateMessage(30, "TCP", "VBS", suffix), exception, details);
                instance.Write("scope", LogLevel.Message, this.CreateMessage(30, "TCP", "MSG", suffix), exception, details);
                instance.Write("scope", LogLevel.Warning, this.CreateMessage(30, "TCP", "WRN", suffix), exception, details);
                instance.Write("scope", LogLevel.Error, this.CreateMessage(30, "TCP", "ERR", suffix), exception, details);
                instance.Write("scope", LogLevel.Fatal, this.CreateMessage(30, "TCP", "FAT", suffix), exception, details);
                instance.Write("scope", LogLevel.Critical, this.CreateMessage(30, "TCP", "CRT", suffix), exception, details);
                instance.Write("scope", LogLevel.Disaster, this.CreateMessage(30, "TCP", "DST", suffix), exception, details);
            }
        }

        [Explicit]
        [TestCase("127.0.0.1", 42021, Address.IPv4, "111")]
        [TestCase("localhost", 42021, Address.IPv4, "222")]
        [TestCase("::1", 42022, Address.IPv6, "333")]
        [TestCase("localhost", 42022, Address.IPv6, "444")]
        public void MessageTcp_StandardWithTermination_MessageSent(String host, Int32 port, Address address, String suffix)
        {
            INetworkLoggerSettings settings = new NetworkLoggerSettings()
            {
                LogLevel = LogLevel.Trace,
                LogType = LogType.Gelf,
                Host = host,
                Port = port,
                Address = address,
                Protocol = Protocol.Tcp,
                ShowKey = false,
                ShowTime = false,
                Termination = true
            };

            Exception exception = this.CreateException();
            (String Label, Object Value)[] details = this.CreateDetails();

            using (INetworkLogger instance = this.CreateDefault(settings))
            {
                instance.Write("scope", LogLevel.Trace, this.CreateMessage(30, "TCP", "TRC", suffix), exception, details);
                instance.Write("scope", LogLevel.Debug, this.CreateMessage(30, "TCP", "DBG", suffix), exception, details);
                instance.Write("scope", LogLevel.Verbose, this.CreateMessage(30, "TCP", "VBS", suffix), exception, details);
                instance.Write("scope", LogLevel.Message, this.CreateMessage(30, "TCP", "MSG", suffix), exception, details);
                instance.Write("scope", LogLevel.Warning, this.CreateMessage(30, "TCP", "WRN", suffix), exception, details);
                instance.Write("scope", LogLevel.Error, this.CreateMessage(30, "TCP", "ERR", suffix), exception, details);
                instance.Write("scope", LogLevel.Fatal, this.CreateMessage(30, "TCP", "FAT", suffix), exception, details);
                instance.Write("scope", LogLevel.Critical, this.CreateMessage(30, "TCP", "CRT", suffix), exception, details);
                instance.Write("scope", LogLevel.Disaster, this.CreateMessage(30, "TCP", "DST", suffix), exception, details);
            }
        }

        [Explicit]
        [TestCase("127.0.0.1", 42021, Address.IPv4, "111")]
        [TestCase("localhost", 42021, Address.IPv4, "222")]
        [TestCase("::1", 42022, Address.IPv6, "333")]
        [TestCase("localhost", 42022, Address.IPv6, "444")]
        public void MessageTcp_ContextNoTermination_MessageSent(String host, Int32 port, Address address, String suffix)
        {
            INetworkLoggerSettings settings = new NetworkLoggerSettings()
            {
                LogLevel = LogLevel.Trace,
                LogType = LogType.Gelf,
                Host = host,
                Port = port,
                Address = address,
                Protocol = Protocol.Tcp,
                ShowKey = false,
                ShowTime = false,
                Termination = false
            };

            Exception exception = this.CreateException();
            (String Label, Object Value)[] details = this.CreateDetails();

            using (INetworkLogger<ContextTestClass> instance = this.CreateContext(settings))
            {
                instance.Write("scope", LogLevel.Trace, this.CreateMessage(30, "TCP", "TRC", suffix), exception, details);
                instance.Write("scope", LogLevel.Debug, this.CreateMessage(30, "TCP", "DBG", suffix), exception, details);
                instance.Write("scope", LogLevel.Verbose, this.CreateMessage(30, "TCP", "VBS", suffix), exception, details);
                instance.Write("scope", LogLevel.Message, this.CreateMessage(30, "TCP", "MSG", suffix), exception, details);
                instance.Write("scope", LogLevel.Warning, this.CreateMessage(30, "TCP", "WRN", suffix), exception, details);
                instance.Write("scope", LogLevel.Error, this.CreateMessage(30, "TCP", "ERR", suffix), exception, details);
                instance.Write("scope", LogLevel.Fatal, this.CreateMessage(30, "TCP", "FAT", suffix), exception, details);
                instance.Write("scope", LogLevel.Critical, this.CreateMessage(30, "TCP", "CRT", suffix), exception, details);
                instance.Write("scope", LogLevel.Disaster, this.CreateMessage(30, "TCP", "DST", suffix), exception, details);
            }
        }

        [Explicit]
        [TestCase("127.0.0.1", 42021, Address.IPv4, "111")]
        [TestCase("localhost", 42021, Address.IPv4, "222")]
        [TestCase("::1", 42022, Address.IPv6, "333")]
        [TestCase("localhost", 42022, Address.IPv6, "444")]
        public void MessageTcp_ContextWithTermination_MessageSent(String host, Int32 port, Address address, String suffix)
        {
            INetworkLoggerSettings settings = new NetworkLoggerSettings()
            {
                LogLevel = LogLevel.Trace,
                LogType = LogType.Gelf,
                Host = host,
                Port = port,
                Address = address,
                Protocol = Protocol.Tcp,
                ShowKey = false,
                ShowTime = false,
                Termination = true
            };

            Exception exception = this.CreateException();
            (String Label, Object Value)[] details = this.CreateDetails();

            using (INetworkLogger<ContextTestClass> instance = this.CreateContext(settings))
            {
                instance.Write("scope", LogLevel.Trace, this.CreateMessage(30, "TCP", "TRC", suffix), exception, details);
                instance.Write("scope", LogLevel.Debug, this.CreateMessage(30, "TCP", "DBG", suffix), exception, details);
                instance.Write("scope", LogLevel.Verbose, this.CreateMessage(30, "TCP", "VBS", suffix), exception, details);
                instance.Write("scope", LogLevel.Message, this.CreateMessage(30, "TCP", "MSG", suffix), exception, details);
                instance.Write("scope", LogLevel.Warning, this.CreateMessage(30, "TCP", "WRN", suffix), exception, details);
                instance.Write("scope", LogLevel.Error, this.CreateMessage(30, "TCP", "ERR", suffix), exception, details);
                instance.Write("scope", LogLevel.Fatal, this.CreateMessage(30, "TCP", "FAT", suffix), exception, details);
                instance.Write("scope", LogLevel.Critical, this.CreateMessage(30, "TCP", "CRT", suffix), exception, details);
                instance.Write("scope", LogLevel.Disaster, this.CreateMessage(30, "TCP", "DST", suffix), exception, details);
            }
        }

        #endregion

        #region WEB

        [Explicit]
        [Test]
        public void MessageWeb_MinimalShowTimeAndTimeoutOneHundred_MessageSent()
        {
            INetworkLoggerSettings settings = new NetworkLoggerSettings()
            {
                LogLevel = LogLevel.Trace,
                LogType = LogType.Gelf,
                Host = "http://localhost/foo/bar",
                Port = 42031,
                Address = Address.Unknown,
                Protocol = Protocol.Web,
                ShowKey = false,
                ShowTime = true,
                Termination = false,
                Timeout = 100
            };

            using (INetworkLogger instance = this.CreateDefault(settings))
            {
                instance.Write(LogLevel.Trace, this.CreateMessage(30, "WEB", "TRC", "111"));
                instance.Write(LogLevel.Debug, this.CreateMessage(30, "WEB", "DBG", "111"));
                instance.Write(LogLevel.Verbose, this.CreateMessage(30, "WEB", "VBS", "111"));
                instance.Write(LogLevel.Message, this.CreateMessage(30, "WEB", "MSG", "111"));
                instance.Write(LogLevel.Warning, this.CreateMessage(30, "WEB", "WRN", "111"));
                instance.Write(LogLevel.Error, this.CreateMessage(30, "WEB", "ERR", "111"));
                instance.Write(LogLevel.Fatal, this.CreateMessage(30, "WEB", "FAT", "111"));
                instance.Write(LogLevel.Critical, this.CreateMessage(30, "WEB", "CRT", "111"));
                instance.Write(LogLevel.Disaster, this.CreateMessage(30, "WEB", "DST", "111"));
            }
        }

        [Explicit]
        [Test]
        public void MessageWeb_MinimalNoShowTimeAndTimeoutOneHundred_MessageSent()
        {
            INetworkLoggerSettings settings = new NetworkLoggerSettings()
            {
                LogLevel = LogLevel.Trace,
                LogType = LogType.Gelf,
                Host = "http://localhost/foo/bar",
                Port = 42031,
                Address = Address.Unknown,
                Protocol = Protocol.Web,
                ShowKey = false,
                ShowTime = false,
                Termination = false,
                Timeout = 100
            };

            using (INetworkLogger instance = this.CreateDefault(settings))
            {
                instance.Write(LogLevel.Trace, this.CreateMessage(30, "WEB", "TRC", "111"));
                instance.Write(LogLevel.Debug, this.CreateMessage(30, "WEB", "DBG", "111"));
                instance.Write(LogLevel.Verbose, this.CreateMessage(30, "WEB", "VBS", "111"));
                instance.Write(LogLevel.Message, this.CreateMessage(30, "WEB", "MSG", "111"));
                instance.Write(LogLevel.Warning, this.CreateMessage(30, "WEB", "WRN", "111"));
                instance.Write(LogLevel.Error, this.CreateMessage(30, "WEB", "ERR", "111"));
                instance.Write(LogLevel.Fatal, this.CreateMessage(30, "WEB", "FAT", "111"));
                instance.Write(LogLevel.Critical, this.CreateMessage(30, "WEB", "CRT", "111"));
                instance.Write(LogLevel.Disaster, this.CreateMessage(30, "WEB", "DST", "111"));
            }
        }

        [Explicit]
        [Test]
        public void MessageWeb_StandardShowTimeAndTimeoutOneHundred_MessageSent()
        {
            INetworkLoggerSettings settings = new NetworkLoggerSettings()
            {
                LogLevel = LogLevel.Trace,
                LogType = LogType.Gelf,
                Host = "http://localhost/foo/bar",
                Port = 42031,
                Address = Address.Unknown,
                Protocol = Protocol.Web,
                ShowKey = false,
                ShowTime = true,
                Termination = false,
                Timeout = 100
            };

            Exception exception = this.CreateException();
            (String Label, Object Value)[] details = this.CreateDetails();

            using (INetworkLogger instance = this.CreateDefault(settings))
            {
                instance.Write(LogLevel.Trace, this.CreateMessage(30, "WEB", "TRC", "111"), exception, details);
                instance.Write(LogLevel.Debug, this.CreateMessage(30, "WEB", "DBG", "111"), exception, details);
                instance.Write(LogLevel.Verbose, this.CreateMessage(30, "WEB", "VBS", "111"), exception, details);
                instance.Write(LogLevel.Message, this.CreateMessage(30, "WEB", "MSG", "111"), exception, details);
                instance.Write(LogLevel.Warning, this.CreateMessage(30, "WEB", "WRN", "111"), exception, details);
                instance.Write(LogLevel.Error, this.CreateMessage(30, "WEB", "ERR", "111"), exception, details);
                instance.Write(LogLevel.Fatal, this.CreateMessage(30, "WEB", "FAT", "111"), exception, details);
                instance.Write(LogLevel.Critical, this.CreateMessage(30, "WEB", "CRT", "111"), exception, details);
                instance.Write(LogLevel.Disaster, this.CreateMessage(30, "WEB", "DST", "111"), exception, details);
            }
        }

        [Explicit]
        [Test]
        public void MessageWeb_ContextShowTimeAndTimeoutOneHundred_MessageSent()
        {
            INetworkLoggerSettings settings = new NetworkLoggerSettings()
            {
                LogLevel = LogLevel.Trace,
                LogType = LogType.Gelf,
                Host = "http://localhost/foo/bar",
                Port = 42031,
                Address = Address.Unknown,
                Protocol = Protocol.Web,
                ShowKey = false,
                ShowTime = true,
                Termination = false,
                Timeout = 100
            };

            Exception exception = this.CreateException();
            (String Label, Object Value)[] details = this.CreateDetails();

            using (INetworkLogger<ContextTestClass> instance = this.CreateContext(settings))
            {
                instance.Write(LogLevel.Trace, this.CreateMessage(30, "WEB", "TRC", "111"), exception, details);
                instance.Write(LogLevel.Debug, this.CreateMessage(30, "WEB", "DBG", "111"), exception, details);
                instance.Write(LogLevel.Verbose, this.CreateMessage(30, "WEB", "VBS", "111"), exception, details);
                instance.Write(LogLevel.Message, this.CreateMessage(30, "WEB", "MSG", "111"), exception, details);
                instance.Write(LogLevel.Warning, this.CreateMessage(30, "WEB", "WRN", "111"), exception, details);
                instance.Write(LogLevel.Error, this.CreateMessage(30, "WEB", "ERR", "111"), exception, details);
                instance.Write(LogLevel.Fatal, this.CreateMessage(30, "WEB", "FAT", "111"), exception, details);
                instance.Write(LogLevel.Critical, this.CreateMessage(30, "WEB", "CRT", "111"), exception, details);
                instance.Write(LogLevel.Disaster, this.CreateMessage(30, "WEB", "DST", "111"), exception, details);
            }
        }

        #endregion

        #region Private helpers

        private INetworkLogger CreateDefault(INetworkLoggerSettings settings)
        {
            return new NetworkLogger(settings);
        }

        private INetworkLogger<ContextTestClass> CreateContext(INetworkLoggerSettings settings)
        {
            return new NetworkLogger<ContextTestClass>(settings);
        }

        private String CreateMessage(Int32 length, String prefix, String level, String suffix)
        {
            String template = "message";

            Int32 count = length / (template.Length + 1) + 1;

            String message = String.Join(" ", Enumerable.Repeat(template, count));

            message = message.Substring(0, Math.Max(0, length - (prefix.Length + level.Length + suffix.Length + 3)));

            return String.Format("{0} {1} {2} {3}", prefix, level, message, suffix);
        }

        private (String Label, Object Value)[] CreateDetails()
        {
            return new (String Label, Object Value)[]
            {
                ("integer", 4711),
                ("string", "value"),
                ("boolean", true),
                ("datetime", new DateTime(2022,10,29,17,23,05,123))
            };
        }

        private Exception CreateException()
        {
            try
            {
                (new ExceptionTestClass()).ThrowException();
                return null;
            }
            catch (Exception exception)
            {
                return exception;
            }
        }

        private class ContextTestClass { }

        private class ExceptionTestClass
        {
            public void ThrowException()
            {
                this.ThrowExceptionStageOne();
            }

            private void ThrowExceptionStageOne()
            {

                this.ThrowExceptionStageTwo();
            }

            private void ThrowExceptionStageTwo()
            {
                throw new Exception("Simple test exception.");
            }
        }

        #endregion
    }
}
