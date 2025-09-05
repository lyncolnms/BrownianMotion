# Desafio de Programação em .NET MAUI

## Introdução

Aplicativo Desktop Windows usando o .NET MAUI para simular e visualizar o movimento browniano geométrico, com base no código fornecido.

O Aplicativo permite que o usuário insira parâmetros como preço inicial, volatilidade, média do retorno e tempo, e então gerar e exibir o gráfico do movimento browniano geométrico.

Também é possível simular múltiplos resultados utilizando o campo `Quantidade de Simulações`.

### Instruções e requisitos

• Implementação da Aplicação .NET MAUI Desktop;

• Utilize o padrão MVVM (Model-View-ViewModel) para organizar a estrutura da aplicação;

• Implemente o código fornecido GenerateBrownianMotion no modelo da sua aplicação;

• Utilizar GraphicsView/IDrawable do .NET MAUI ou SkiaSharp do .NET.;

• Entrada de Parâmetros: Adicione controles de entrada na View para permitir que o usuário insira os seguintes parâmetros:
 
- Preço inicial;

- Volatilidade;

- Média do retorno;

- Tempo (duração do gerador de preços).

### Opcionais

- [x] Adicionar escala vertical e horizontal;

- [x] Permitir que o usuário simule múltiplos resultados (parâmetro na tela informando
número de simulações), plotando mais de uma linha;

- [ ] Enriquecer os componentes de entrada de parâmetro (exemplo, stepper, slider...);

- [x] Enriquecer o visual e layout da aplicação;

- [ ] Permitir que o usuário personalize visualmente o gráfico, por exemplo, escolhendo cores, estilo de linha, etc.;

- [x] Enriquecer a responsividade da aplicação;

- [x] Documentação;

- [ ] Testes unitários (Parcial);

- [x] Utilizar .Net 9.

---

## Imagens da aplicação

![Inicio](docs/assets/Captura%20de%20tela%202025-09-05%20121145.png)

---

![Apenas 1 simulação](docs/assets/Captura%20de%20tela%202025-09-05%20120515.png)
---

![Múltiplas simulações](docs/assets/Captura%20de%20tela%202025-09-05%20120545.png)

---