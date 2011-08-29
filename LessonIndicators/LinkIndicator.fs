// Learn more about F# at http://fsharp.net

// C# tradelink tutorial
// http://code.google.com/p/tradelink/wiki/LessonIndicators
// ported to F#

module LinkIndicator

open TradeLink.API
open TradeLink.Common
open System.IO

type LinkIndicator() as this =
    inherit TradeLink.Common.ResponseTemplate()

    let mutable btl = new TradeLink.Common.BarListTracker()

    let caption = [| "Time"; "AVGHL" |]

    do
        this.Indicators <- [| "Time"; "AVGHL" |]
        this.senddebug("LinkIndicator constructor")

    member x.Btl
        with get() = btl
        and set value = btl <- value

    override this.GotTick(tick : Tick) = (

        this.senddebug("GotTick")

        let minbars = 10
        let maxvol: decimal = 0.5M;
        
        btl.newTick(tick)

        let symbol = tick.symbol

        let item = btl.Item(symbol)
        
        if not (item.Has( minbars )) then

            let highs = item.High()
            let lows = item.Low()
            let hlrange = Calc.Subtract( highs, lows)
            let avghl = Calc.Avg(hlrange)

            if (avghl > maxvol) then
                this.senddebug(tick.symbol + " exceeded max volatility " + tick.time.ToString())
            
            let output1 = [| tick.time.ToString(); avghl.ToString() |]
            this.sendindicators( output1 )

            let price = avghl + item.RecentBar.Close
            let time = item.Last
            this.sendchartlabel( price, time )
            )


