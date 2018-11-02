using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SQ7MRU.FLLog.Model;
using SQ7MRU.FLLog.Requests;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;

namespace SQ7MRU.FLLog.Controllers
{
    [Route("RPC2")]
    public class XmlRpcController : ControllerBase
    {
        private readonly QsoCollectorClient client;

        public XmlRpcController(IConfiguration configuration)
        {
            client = new QsoCollectorClient(configuration);
        }

        [HttpPost]
        public ContentResult Post()
        {
            using (var receiveStream = Request.Body)
            {
                using (var readStream = new StreamReader(receiveStream))
                {
                    var call = XmlHelper.DeserializeFromString<MethodCall>(readStream.ReadToEnd());

                    switch (call.methodName)
                    {
                        #region system.listMethods
                        case "system.listMethods":
                        #endregion
                            {
                                return new ContentResult()
                                {
                                    Content = XmlHelper.SerializeObject<MethodResponse>(new MethodResponse()
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
                                                            Value = new [] {
                                                            "log.add_record",
                                                            "log.check_dup",
                                                            "log.get_record",
                                                            "system.listMethods",
                                                            "system.methodHelp",
                                                            "system.multicall" }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }),
                                    ContentType = MediaTypeNames.Text.Xml,
                                    StatusCode = 200
                                };
                            }

                        #region system.methodHelp
                        case "system.methodHelp":
                            {
                                switch (call.@params?.First().value)
                                {
                                    case "log.add_record":
                                        {
                                            return new ContentResult()
                                            {
                                                Content = XmlHelper.SerializeObject<MethodResponse>(new MethodResponse()
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
                                                                        Value = new[]
                                                                        {"log.add_record ADIF RECORD"}
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }),
                                                ContentType = MediaTypeNames.Text.Xml,
                                                StatusCode = 200
                                            };
                                        }

                                    case "log.check_dup":
                                        {
                                            return new ContentResult()
                                            {
                                                Content = XmlHelper.SerializeObject<MethodResponse>(new MethodResponse()
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
                                                                        Value = new[]
                                                                        {"log.check_dup CALL, MODE(0), TIME_SPAN(0), FREQ_HZ(0), STATE(0), XCHG_IN(0)"}
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }),
                                                ContentType = MediaTypeNames.Text.Xml,
                                                StatusCode = 200
                                            };
                                        }

                                    case "log.get_record":
                                        {
                                            return new ContentResult()
                                            {
                                                Content = XmlHelper.SerializeObject<MethodResponse>(new MethodResponse()
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
                                                                        Value = new[]
                                                                        {"log.get_record CALL"}
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }),
                                                ContentType = MediaTypeNames.Text.Xml,
                                                StatusCode = 200
                                            };
                                        }

                                    default:
                                        return null;
                                }
                            }
                        #endregion

                        #region log.check_dup
                        ///"log.check_dup CALL, MODE(0), TIME_SPAN(0), FREQ_HZ(0), STATE(0), XCHG_IN(0)"
                        case "log.check_dup":
                         {
                                var req = new CheckDupRequest()
                                {
                                    //CALL
                                    Call = call.@params[0].value,

                                    //MODE(0)
                                    Mode = call.@params[1]?.value ?? null,

                                    //TIME_SPAN(0)
                                    TimeSpan = int.Parse(call.@params[2]?.value ?? "10"),

                                    //FREQ_HZ(0)
                                    Freq = call.@params[3]?.value ?? null,

                                    //STATE(0)
                                    State = call.@params[4]?.value ?? null,

                                    //XCHG_IN(0)
                                    XchgIn = call.@params[5]?.value ?? null,
                                };


                                return new ContentResult()
                                {
                                    Content = XmlHelper.SerializeObject<MethodResponse>(new MethodResponse()
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
                                                            Value = new[] {
                                                                client.CheckDup(req).ToString()
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }),
                                    ContentType = MediaTypeNames.Text.Xml,
                                    StatusCode = 200
                                };
                            }
                        #endregion

                        default:
                            return null;
                    }
                }
            }
        }
    }
}