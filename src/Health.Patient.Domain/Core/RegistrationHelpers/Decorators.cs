using Health.Patient.Domain.Core.Decorators;

namespace Health.Patient.Domain.Core.RegistrationHelpers;

public static class Decorators
{
    public static bool IsDecorator(object x)
    {
        return x is LoggingPipelineAttribute || x is ValidationPipelineAttribute || x is TransactionPipelineAttribute || x is ExceptionPipelineAttribute;
    }
    
    public static Type ToDecorator(object attribute, Type assigningInterfaceType)
    {
        Type type = attribute.GetType();

        if (type == typeof(LoggingPipelineAttribute))
        {
            if (Handlers.IsCommandHandlerInterface(assigningInterfaceType))
                return typeof(LoggingCommandDecorator<,>);
            if (Handlers.IsQueryHandlerInterface(assigningInterfaceType))
                return typeof(LoggingQueryDecorator<,>);
        }
        
        if (type == typeof(ValidationPipelineAttribute))
        {
            if (Handlers.IsCommandHandlerInterface(assigningInterfaceType))
                return typeof(ValidationCommandDecorator<,>);
            if (Handlers.IsQueryHandlerInterface(assigningInterfaceType))
                return typeof(ValidationQueryDecorator<,>);
        }

        if (type == typeof(TransactionPipelineAttribute))
        {
            if (Handlers.IsCommandHandlerInterface(assigningInterfaceType))
                return typeof(TransactionCommandDecorator<,>);
            if (Handlers.IsQueryHandlerInterface(assigningInterfaceType))
                throw new ArgumentException(attribute.ToString());
        }

        if (type == typeof(ExceptionPipelineAttribute))
        {
            if (Handlers.IsCommandHandlerInterface(assigningInterfaceType))
                return typeof(ExceptionCommandDecorator<,>);
            if (Handlers.IsQueryHandlerInterface(assigningInterfaceType))
                return typeof(ExceptionQueryDecorator<,>);
        }

        // other attributes go here

        throw new ArgumentException(attribute.ToString());
    }
}