﻿using Microsoft.AspNetCore.Mvc;
using SQ7MRU.FLLog.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SQ7MRU.FLLog.Controllers
{
    [Route("RPC2")]
    public class XmlRpcController : ControllerBase
    {
        [HttpPost]
        [Produces("text/xml")]
        [ResponseCache(NoStore = true)]
        public MethodResponse Post()
        {

            using (var receiveStream = Request.Body)
            {
                using (var readStream = new StreamReader(receiveStream))
                {
                    var call = XmlHelper.XmlDeserializeFromString<MethodCall>(readStream.ReadToEnd());

                    switch (call.methodName)
                    {
                        case "system.listMethods":
                            {
                                return new MethodResponse()
                                {
                                    Params = new Params()
                                    {
                                        Param = new Param()
                                        {
                                            Value = new Value()
                                            {
                                                Array = new Array()
                                                {
                                                    Data = new Data()
                                                    {
                                                        Value = new List<string>() {
                                                            "log.add_record",
                                                            "log.check_dup",
                                                            "log.get_record",
                                                            "system.listMethods",
                                                            "system.methodHelp",
                                                            "system.multicall"
                                                              }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                };
                            }

                        case "system.methodHelp":
                            {
                                switch (call.@params?.First().value)
                                {
                                    case "log.add_record":
                                        {
                                            return new MethodResponse()
                                            {
                                                Params = new Params()
                                                {
                                                    Param = new Param()
                                                    {
                                                        Value = new Value()
                                                        {
                                                            Array = new Array()
                                                            {
                                                                Data = new Data()
                                                                {
                                                                    Value = new List<string>()
                                                                    {
                                                                        "log.add_record ADIF RECORD"
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            };
                                        }

                                    case "log.check_dup":
                                        {
                                            return new MethodResponse()
                                            {
                                                Params = new Params()
                                                {
                                                    Param = new Param()
                                                    {
                                                        Value = new Value()
                                                        {
                                                            Array = new Array()
                                                            {
                                                                Data = new Data()
                                                                {
                                                                    Value = new List<string>()
                                                                    {
                                                                        "log.check_dup CALL, MODE(0), TIME_SPAN(0), FREQ_HZ(0), STATE(0), XCHG_IN(0)"
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            };
                                        }

                                    case "log.get_record":
                                        {
                                            return new MethodResponse()
                                            {
                                                Params = new Params()
                                                {
                                                    Param = new Param()
                                                    {
                                                        Value = new Value()
                                                        {
                                                            Array = new Array()
                                                            {
                                                                Data = new Data()
                                                                {
                                                                    Value = new List<string>()
                                                                    {
                                                                        "log.get_record CALL"
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            };
                                        }

                                    default:
                                        return null;
                                }
                            }

                        default:
                            return null;
                    }
                }
            }
        }
    }
}