// Learn more about F# at http://fsharp.net

module HelloWorld

open TradeLink.API
open TradeLink.Common
open System.IO

type HelloWorld() =
    inherit TradeLink.Common.ResponseTemplate()
        override this.GotTick(tick : Tick) = (
            this.senddebug("Hello F# World")
        )

