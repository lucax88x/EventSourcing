namespace EventSourcing.Domain

open System
open MediatR

module Base =

    type Command() =
        interface IRequest

    type Event(?createdWhen: DateTimeOffset) =
        interface INotification
        member this.CreatedWhen = defaultArg createdWhen DateTimeOffset.UtcNow
            
    [<AbstractClass>]
    type AggregateRoot() = 
        let mutable changes: List<Event> = []
            
        abstract member Apply: Event -> Event

        member val Id = Guid.NewGuid() with get, set

        member this.Changes
            with get () = changes
            and private set (value) = changes <- value

        member this.ApplyChange(event: Event) =
            this.Apply(event) |> ignore
            this.Changes <- this.Changes @ [event]
            this

        member this.GetUncommittedChanges() =
            this.Changes
        member this.MarkChangesAsCommitted() =
            this.Changes = []
            |> ignore
        member this.LoadsFromHistory(events: seq<Event>) =
            events
            |> Seq.iter (fun e -> this.ApplyChange(e) |> ignore)