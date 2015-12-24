using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport
{
    class ErrorEventsRedirector
    {
        LogicPartsKeeper logicParts = LogicPartsKeeper.getInstance();
        public delegate void handler(object sender, Exception e);
        List<ICrudErrorable> EntitiesWithErrorEvents;

        EventHandler<Exception> AllSelectedErrorEvent;
        EventHandler<Exception> DeleteErrorEvent;
        EventHandler<Exception> EditErrorEvent;
        EventHandler<Exception> InsertErrorEvent;
        EventHandler<Exception> SelectionErrorEvent;
        public ErrorEventsRedirector(EventHandler<Exception> AllSelectedErrorEvent,
                                     EventHandler<Exception> DeleteErrorEvent,
                                     EventHandler<Exception> EditErrorEvent,
                                     EventHandler<Exception> InsertErrorEvent,
                                     EventHandler<Exception> SelectionErrorEvent
                                    ) {
            EntitiesWithErrorEvents = new List<ICrudErrorable>();

            this.AllSelectedErrorEvent = AllSelectedErrorEvent;
            this.DeleteErrorEvent = DeleteErrorEvent;
            this.EditErrorEvent = EditErrorEvent;
            this.InsertErrorEvent = InsertErrorEvent;
            this.SelectionErrorEvent = SelectionErrorEvent;
            
        }
        public void Subscribe()
        {
            foreach (Logic logic in logicParts.LogicPartsList)
            {
                EntitiesWithErrorEvents.Add(logic);
            }
            foreach (ICrudErrorable events in EntitiesWithErrorEvents)
            {
                if(AllSelectedErrorEvent!=null)
                events.AllSelectedError += (sender, exception) => AllSelectedErrorEvent(sender, exception);
                if (DeleteErrorEvent != null)
                events.DeleteError += (sender, exception) => DeleteErrorEvent(sender, exception);
                if (EditErrorEvent != null)
                events.EditError += (sender, exception) => EditErrorEvent(sender, exception);
                if (InsertErrorEvent != null)
                events.InsertionError += (sender, exception) => InsertErrorEvent(sender, exception);
                if (SelectionErrorEvent != null)
                events.SelectionError += (sender, exception) => SelectionErrorEvent(sender, exception);
            }
        }
    }
}
