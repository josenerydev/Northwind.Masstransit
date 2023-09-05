# Convenções no MassTransit: Uma Discussão Detalhada com Exemplos

## O Que São Convenções no MassTransit?

No contexto do MassTransit, as convenções são recursos usados para mapear tipos de mensagens a endereços de endpoints. Uma das principais razões para usar convenções é reduzir a verbosidade associada a obter o "send endpoint" toda vez que uma mensagem precisa ser enviada.

Um trecho típico, sem usar convenções, seria assim:

```csharp
var endpoint = await bus.GetSendEndpoint(new Uri("rabbitmq://mq.acme.com/order/order_processing"));
await endpoint.Send(new SubmitOrder{ /* ... */ });
```

Com convenções, isso pode ser simplificado. Primeiro, você configura a convenção:

```csharp
EndpointConvention.Map<SubmitOrder>(new Uri("rabbitmq://mq.acme.com/order/order_processing"));
```

Depois, envia a mensagem sem ter que obter o send endpoint:

```csharp
await _bus.Send(new SubmitOrder{ /* ... */ });
```

## Vantagens e Desvantagens

### Vantagens:

1. **Menos verbosidade**: Não é necessário obter o "send endpoint" todas as vezes.
2. **Separação da Configuração**: Não é preciso espalhar endereços de endpoint por toda a aplicação. Ao usar convenções, o mapeamento entre tipos de mensagens e endereços de endpoints é centralizado.

### Desvantagens:

1. **Restrição de Mapeamento Único**: Não é possível enviar mensagens do mesmo tipo para diferentes endpoints usando convenções.
2. **Erro em Múltiplas Configurações**: Tentar configurar convenções para a mesma mensagem mais de uma vez resultará em exceção.

## Cuidados ao Usar `Publish` para Comandos

Ao explorar a funcionalidade do MassTransit, encontramos essa característica que permite rotear mensagens por tipo. Esta funcionalidade, conhecida como "Publish", é altamente eficaz. Porém, vale lembrar que estamos falando de um "command", e comandos tradicionalmente devem ser "Sent". Contudo, se você tem controle sobre a aplicação e sabe que há apenas um consumer para a mensagem "KickTheTiresAndLightTheFires", então usar "publish" pode ser tão eficaz quanto "send". O benefício é que você não precisa conhecer o endereço!

No entanto, é crucial ser cauteloso ao tomar essa decisão, já que existem algumas implicações:

1. **Perda de Mensagens**: Se o consumer bus não for iniciado antes do producer, o "command" publicado será perdido.
2. **Processamento Múltiplo**: Se o "command" for publicado, é possível que o mesmo seja processado várias vezes, caso você tenha múltiplos consumers para a mesma mensagem.
3. **Mais Exchange/Topic**: Para cada "command" que você publicar, um exchange ou topic adicional será criado.

## Exemplo Usando `IEndpointNameFormatter`

Suponha que você queira gerar nomes de receive endpoints de forma dinâmica ou seguindo uma certa convenção específica em sua aplicação. O MassTransit permite isso usando `IEndpointNameFormatter`.

Primeiro, você precisa implementar sua própria lógica para nomear:

```csharp
public class MyEndpointNameFormatter : IEndpointNameFormatter
{
    public string Consumer<T>()
    {
        // Sua lógica personalizada aqui
        return typeof(T).Name + "_queue";
    }

    // Implemente outros métodos conforme necessário...
}
```

Depois, você pode usar essa implementação para gerar o nome do endpoint e registrar sua convenção:

```csharp
var formatter = new MyEndpointNameFormatter();
string queueName = formatter.Consumer<SubmitOrder>();
EndpointConvention.Map<SubmitOrder>(new Uri($"queue:{queueName}"));
```

Assim, a mensagem `SubmitOrder` será enviada para o endpoint nomeado "SubmitOrder_queue" (ou qualquer lógica que você definiu).

Chris Patterson, uma figura proeminente por trás do MassTransit, esclareceu que a `EndpointConvention` é um método de conveniência e não é configurado automaticamente. Ele apontou que se cada tipo de mensagem fosse registrado como uma convenção de endpoint, haveria problemas ao lidar com mensagens que são publicadas e consumidas em múltiplos endpoints.

Ele sugere que, se você realmente quer usar `Send`, pode usar o `IEndpointNameFormatter` (como mostrado acima) para gerar os nomes dos receive endpoints e registrar suas próprias convenções de endpoint.

Em resumo, enquanto as convenções de endpoint no MassTransit podem parecer uma solução atrativa para simplificar o envio de mensagens, é crucial entender suas implicações. Se mal utilizadas, podem introduzir erros inesperados ou comportamentos indesejados em sua aplicação. Ao usar o `Publish` para comandos, o mesmo cuidado deve ser tomado para evitar tais problemas. Como em muitas ferramentas poderosas, o entendimento completo e o uso criterioso são essenciais.