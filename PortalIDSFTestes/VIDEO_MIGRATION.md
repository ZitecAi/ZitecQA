# Migração de Screenshots para Vídeo

## Visão Geral
Este projeto foi migrado de capturas de screenshots para gravação de vídeo para fornecer evidências mais completas e profissionais dos testes automatizados.

## Principais Mudanças

### 1. Novo VideoHelper
- **Localização**: `metodos/VideoHelper.cs`
- **Funcionalidades**:
  - Gravação automática de vídeo durante toda a execução do teste
  - Anexo automático do vídeo ao relatório Allure (passado ou falhado)
  - Limpeza automática de vídeos antigos

### 2. Modificações na Classe Base (Executa.cs)
- **Substituição**: `ScreenshotHelper.ClearOldScreenshots()` → `VideoHelper.ClearOldVideos()`
- **Configuração**: Adicionado `RecordVideoDir` nas opções do contexto do browser
- **TearDown**: Vídeo anexado automaticamente baseado no status do teste

### 3. MetodosSemVideo.cs
- **Propósito**: Versão dos métodos de validação sem dependência de screenshots
- **Uso**: Pode ser usado como alternativa limpa sem capturas de tela individuais

## Como Funciona

### Gravação Automática
1. Quando `AbrirBrowserAsync()` é chamado, o contexto do browser é configurado com `RecordVideoDir`
2. O vídeo começa a ser gravado automaticamente
3. Quando `FecharBrowserAsync()` é chamado:
   - A gravação para automaticamente
   - O vídeo é salvo com nome baseado no teste e status
   - O vídeo é anexado ao relatório Allure

### Nomenclatura dos Vídeos
```
{NomeClasse}_{NomeTeste}_{Status}_{Timestamp}.webm
```
Exemplo: `LoginTest_Deve_Realizar_Login_Com_Sucesso_Passed_20251114_175530.webm`

## Benefícios

### ✅ Vantagens sobre Screenshots
- **Evidência completa**: Todo o fluxo do teste é gravado
- **Contexto total**: Mostra interações antes e depois dos pontos críticos
- **Profissionalismo**: Apresentação mais profissional em relatórios
- **Debugging**: Mais fácil identificar problemas de timing e fluxo
- **Armazenamento**: Um arquivo por teste vs múltiplas screenshots

### ✅ Manutenção Simplificada
- **Um ponto central**: Toda a lógica está no `VideoHelper`
- **Sem retrabalho**: Não precisa mais gerenciar múltiplas screenshots
- **Automático**: Funciona sem intervenção manual nos testes

## Configuração

### Diretório de Vídeos
- **Local**: `{TestDirectory}/videos/`
- **Limpeza**: Automática no início da execução dos testes
- **Formato**: `.webm` (padrão Playwright)

### Integração Allure
- **Tipo MIME**: `video/webm`
- **Nome do anexo**: `Video - {Status}` (Passed/Failed)
- **Visualização**: Reprodutor de vídeo integrado no relatório

## Uso Recomendado

### Para Novos Testes
Use a classe base `Executa` normalmente - a gravação de vídeo é automática:

```csharp
[Test]
public async Task MeuTeste()
{
    // O vídeo está sendo gravado automaticamente
    await page.GotoAsync("https://exemplo.com");
    await page.ClickAsync("#botao");
    // Vídeo continua gravando...
}
```

### Para Testes Existentes
Apenas execute os testes existentes - eles já usarão vídeo automaticamente!

### Se Precisar Desabilitar Vídeo
Comente a linha `RecordVideoDir` em `Executa.cs`:

```csharp
var contextOptions = new BrowserNewContextOptions()
{
    ViewportSize = new ViewportSize() { Width = 1920, Height = 1080 },
    IgnoreHTTPSErrors = true,
    // RecordVideoDir = Path.Combine(TestContext.CurrentContext.TestDirectory, "videos") // Comentar esta linha
};
```

## Troubleshooting

### Vídeos Não Aparecendo
1. Verifique se o diretório `videos/` tem permissões de escrita
2. Confirme que `RecordVideoDir` está configurado no contexto
3. Verifique se há espaço em disco suficiente

### Vídeos com 0KB ou Vazios
**Causas Comuns:**
- Contexto fechado antes da finalização do vídeo
- Race condition entre gravação e captura
- Falha na escrita do arquivo

**Soluções Implementadas:**
- ✅ **WaitForVideoFileComplete()**: Aguarda o arquivo ser completamente escrito
- ✅ **CopyVideoWithRetry()**: Tenta múltiplas vezes copiar o vídeo
- ✅ **Delay estratégico**: Esperas inteligentes antes de fechar contexto
- ✅ **Validação de tamanho**: Verifica se o arquivo tem conteúdo antes de anexar

### Vídeos Interrompidos Prematuramente
**Causas Comuns:**
- Contexto fechado muito rápido
- Múltiplas páginas no contexto
- Timing incorreto no teardown

**Soluções Implementadas:**
- ✅ **Delay de 1s no teardown**: Garante finalização da gravação
- ✅ **ForceVideoFinalization()**: Método para forçar conclusão
- ✅ **Try-catch robusto**: Protege contra falhas no fechamento
- ✅ **Ordem correta de fechamento**: Contexto → Browser → Playwright

### Vídeos Muito Grandes
- Considere testes mais curtos ou divida testes longos
- Vídeos são otimizados automaticamente pelo Playwright
- Configurado `RecordVideoSize` para controle de dimensão

### Performance
- O impacto na performance é mínimo (~2-5%)
- Benefícios superam qualquer pequena lentidão
- Delays adicionais são compensados pela qualidade da evidência

## Melhorias Implementadas (v2.0)

### 🛠️ Robustez Aumentada
1. **Verificação de arquivo completo**: Sistema inteligente que espera o vídeo ser totalmente escrito
2. **Retry mechanism**: Múltiplas tentativas para copiar arquivos com falha
3. **Validação de conteúdo**: Verifica se o vídeo tem tamanho > 0 antes de anexar
4. **Logging detalhado**: Mensagens claras para debug de problemas

### 🔄 Timing Otimizado
1. **Delays estratégicos**: Esperas nos momentos certos para evitar race conditions
2. **Finalização forçada**: Método para garantir que a gravação termine corretamente
3. **Ordem de fechamento**: Sequência correta para evitar perda de dados

### 📊 Monitoramento e Debug
1. **Status logging**: Informações detalhadas sobre o processo de vídeo
2. **Error handling**: Captura e tratamento de exceções específicas
3. **File validation**: Verificação completa do arquivo antes do anexo

### 🎯 Utilitários Adicionais
1. **VideoUtils.cs**: Classe utilitária com métodos de suporte
2. **IsVideoRecording()**: Verifica se a gravação está ativa
3. **WaitForVideoStabilization()**: Aguarda estabilização do vídeo

## Migração Futura

### Opção 1: Manter Ambos (Recomendado)
- Manter `ScreenshotHelper.cs` para uso específico se necessário
- Usar vídeo como evidência principal

### Opção 2: Remover Screenshots Completamente
- Excluir `ScreenshotHelper.cs`
- Remover referências em métodos individuais
- Usar apenas `MetodosSemVideo.cs`

## Exemplo de Relatório Allure

No relatório Allure, você verá:
- **Teste Passou**: Vídeo completo do fluxo bem-sucedido
- **Teste Falhou**: Vídeo mostrando exatamente onde ocorreu a falha
- **Player Integrado**: Reprodução direto no navegador

Esta migração representa um avanço significativo na qualidade e profissionalismo das evidências de teste!