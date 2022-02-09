using Health.Patient.Domain.Core.Decorators;

namespace Health.Patient.Domain.Core.RegistrationHelpers;

public static class Decorators
{
    public static bool IsDecorator(object x)
    {
        return x is AuditLogPipelineAttribute || x is ValidationPipelineAttribute || x is TransactionPipelineAttribute;
    }
    
    public static Type ToDecorator(object attribute, Type assigningInterfaceType)
    {
        Type type = attribute.GetType();

        if (type == typeof(AuditLogPipelineAttribute))
        {
            if (Handlers.IsCommandHandlerInterface(assigningInterfaceType))
                return typeof(AuditLoggingCommandDecorator<,>);
            if (Handlers.IsQueryHandlerInterface(assigningInterfaceType))
                return typeof(AuditLoggingQueryDecorator<,>);
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

        // other attributes go here

        throw new ArgumentException(attribute.ToString());
    }
}