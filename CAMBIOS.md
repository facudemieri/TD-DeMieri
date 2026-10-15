# Cambios: estructura PN1/PN2 y entidades del mantenimiento de aeronaves

La solución compila al terminar cada una de las cinco partes. Al final quedó sin errores y sin advertencias. No se usó git, no se ejecutó la aplicación ni nada contra la base de datos, y no se modificó ningún archivo .Designer.cs ni .resx.

## Parte 1: carpetas PN1 y PN2

Cuando empecé, las carpetas PN1 y PN2 ya existían en los tres proyectos (BE_23DB, Mapper_23DB y BLL_23DB). Los archivos del PN1 ya estaban adentro de su carpeta PN1, pero los tres .csproj los seguían buscando en la raíz del proyecto. Lo que hice fue actualizar los .csproj para que apunten a las rutas nuevas, sin mover nada a mano.

Quedaron así:

- **BE_23DB/PN1:** las ocho entidades Asiento_62FD, Factura_62FD, Pago_62FD, Pasaje_62FD, Pasajero_62FD, Reserva_62FD, ReservaDetalle_62FD y Vuelo_62FD.
- **Mapper_23DB/PN1:** los siete mappers mapperAsiento_62FD, mapperFactura_62FD, mapperPago_62FD, mapperPasaje_62FD, mapperPasajero_62FD, mapperReserva_62FD y mapperVuelo_62FD. ReservaDetalle no tiene mapper propio, como ya pasaba antes.
- **BLL_23DB/PN1:** las siete clases AsientoBLL_62FD, FacturaBLL_62FD, PagoBLL_62FD, PasajeBLL_62FD, PasajeroBLL_62FD, ReservaBLL_62FD y VueloBLL_62FD.

Revisé los namespaces y no cambiaron: siguen siendo BE_23DB, Mapper_23DB y BLL_23DB, sin el nombre de la carpeta.

En estos tres proyectos no había ningún archivo del módulo de seguridad con sufijo _23DB, así que no había nada que dejar en la raíz. Usuario, Rol, Patente, Familia, Evento, DV y sus mappers y BLL están en el proyecto Services_23DB (por ejemplo en su carpeta Composite), y ahí no toqué nada.

## Parte 2: las seis entidades del PN2

Las seis clases están en BE_23DB/PN2, con namespace BE_23DB, y quedaron registradas en BE_23DB.csproj. Siguen el estilo de Reserva_62FD: campo privado declarado aparte, propiedad pública con get y set, los mismos using y el mismo formato de archivo.

**Aeronave_62FD** tiene Matricula_62FD (string), NroSerie_62FD (string), Modelo_62FD (string), Fabricante_62FD (string), EstadoOperativo_62FD (string), HorasVueloAcumuladas_62FD (int), IntervaloRevision_62FD (int) y FechaProximaRevision_62FD (DateTime).

**OrdenTrabajo_62FD** tiene IdOrden_62FD (int), TipoIntervencion_62FD (string), Descripcion_62FD (string), EstadoOrden_62FD (string), FechaInicioPlanificada_62FD (DateTime), FechaLimite_62FD (DateTime), Hangar_62FD (string) y LegajoTecnico_62FD (string).

**Intervencion_62FD** tiene IdIntervencion_62FD (int), estadoIntervencion_62FD (string), fechaHoraInicio_62FD (DateTime), fechaHoraFin_62FD (DateTime) y totalHorasInsumidas_62FD (decimal).

**Tarea_62FD** tiene IdTarea_62FD (int), Descripcion_62FD (string), HorasInsumidas_62FD (decimal), observacionesTecnicas_62FD (string) y ResultadoObtenido_62FD (string).

**Repuesto_62FD** tiene nroSolicitud_62FD (int), nroParte_62FD (string), nombreRepuesto_62FD (string), cantidadSolicitada_62FD (int), estadoPedido_62FD (string) y fechaSolicitud_62FD (DateTime).

**Certificacion_62FD** (sin tilde) tiene codHabilitante_62FD (string), fechaHoraCertificacion_62FD (DateTime), resultadoInspeccion_62FD (string) y Observaciones_62FD (string).

Los nombres que empiezan con minúscula se respetaron tal como vienen del diagrama. Eso generó un problema con los campos privados. La convención del proyecto es nombrar el campo como la propiedad pero con la primera letra en minúscula, y en estas propiedades el campo terminaría llamándose igual que la propiedad, cosa que no compila. En esos casos el campo lleva un guion bajo adelante: por ejemplo, la propiedad estadoIntervencion_62FD usa el campo _estadoIntervencion_62FD. Pasa en todas las propiedades que arrancan en minúscula. Son campos privados, así que no aparecen en el diagrama de clases ni en ningún otro lado de afuera, pero es la única excepción al estilo del proyecto y te la marco.

## Claves foráneas y propiedades de navegación agregadas

Estas son todas las propiedades que agregué además de los atributos del diagrama. En cada clase van al final, después de un comentario que dice "asociaciones del diagrama conceptual". Hay que reflejarlas en el DER.

- **OrdenTrabajo_62FD.Matricula_62FD** (string): clave foránea a Aeronave. Corresponde a la asociación "Requiere", Aeronave 1 a 0..* OrdenTrabajo.
- **Intervencion_62FD.IdOrden_62FD** (int): clave foránea a OrdenTrabajo. Corresponde a "Origina", OrdenTrabajo 1 a 1 Intervencion.
- **Tarea_62FD.IdIntervencion_62FD** (int): clave foránea a Intervencion. Corresponde a "Contiene", Intervencion 1 a 1..* Tarea.
- **Intervencion_62FD.Tareas_62FD** (List de Tarea_62FD): navegación de la misma asociación "Contiene".
- **Certificacion_62FD.IdIntervencion_62FD** (int): clave foránea a Intervencion. Corresponde a "Recibe", Intervencion 1 a 0..1 Certificacion.
- **Repuesto_62FD.IdTarea_62FD** (int): clave foránea a Tarea. Corresponde a "Utiliza", Tarea 1 a 0..* Repuesto.
- **Tarea_62FD.Repuestos_62FD** (List de Repuesto_62FD): navegación de la misma asociación "Utiliza".

Las dos listas se inicializan en la declaración del campo, igual que los detalles de Reserva_62FD, así que nunca quedan en null. No agregué ninguna otra propiedad.

Para el DER hay un detalle a tener en cuenta. La clase no puede garantizar las multiplicidades 1 a 1 de "Origina" y 0..1 de "Recibe": que una orden tenga una sola intervención y que una intervención tenga una sola certificación se tiene que asegurar en la base, con una restricción UNIQUE sobre IdOrden en la tabla de intervenciones y sobre IdIntervencion en la de certificaciones. Lo mismo pasa con el mínimo de una tarea por intervención (1..*): no lo garantiza ni la clase ni una clave foránea, y lo va a tener que validar la BLL.

## Observaciones sobre los DateTime

**fechaHoraFin_62FD** de Intervencion_62FD es DateTime y no admite valor vacío. Mientras la intervención está en curso todavía no hay fecha de fin, y el valor que queda es DateTime.MinValue, que se muestra como 01/01/0001. Eso se va a ver en grillas y reportes. Además, si el mapper la guarda tal cual, SQL Server la rechaza en una columna datetime, porque ese tipo empieza en 1753 (datetime2 sí la acepta). Lo dejé como DateTime para que coincida con el diagrama. Si pasa a DateTime?, la columna en la base tendría que admitir NULL.

**FechaProximaRevision_62FD** de Aeronave_62FD tiene el mismo problema. En una aeronave recién dada de alta que todavía no tiene revisión calculada, queda en 01/01/0001. La dejé como DateTime y la decisión es tuya.

## Parte 3: clases vacías de Mapper y de BLL

En Mapper_23DB/PN2 están mapperAeronave_62FD, mapperOrdenTrabajo_62FD, mapperIntervencion_62FD, mapperTarea_62FD, mapperRepuesto_62FD y mapperCertificacion_62FD. En BLL_23DB/PN2 están AeronaveBLL_62FD, OrdenTrabajoBLL_62FD, IntervencionBLL_62FD, TareaBLL_62FD, RepuestoBLL_62FD y CertificacionBLL_62FD. Siguen la misma convención de nombres que el PN1.

Cada archivo tiene solamente el namespace del proyecto y la declaración de la clase pública vacía. No tienen using, campos, constructores ni métodos. Las doce quedaron registradas en su .csproj.

## Parte 4: comentarios "// nuevo"

Revisé todos los archivos .cs del proyecto, excepto los .Designer.cs, que en la tanda anterior no se habían tocado. En total había 211 marcadores en 15 archivos, todos de la capa GUI, salvo uno en IdiomaBLL_23DB de Services_23DB.

Borré 187 por completo. De esos, 176 estaban al final de una línea de código (en esos casos borré solo el comentario) y 11 ocupaban una línea propia (ahí borré la línea entera).

Los otros 24 tenían una explicación. A esos les saqué la palabra "nuevo" y la explicación quedó como un comentario normal:

- **AuditoriaEventos_23DB**, en las dos cargas de la grilla: "las columnas autogeneradas se recrean en cada carga".
- **CambiarIdioma_23DB**: "la sesión se actualiza ANTES de notificar, para que ObtenerTexto_23DB ya devuelva el idioma nuevo…". Además de sacar la palabra, le borré las palabras "orden invertido", que hablaban del cambio y no del código.
- **SeleccionarVuelo_62FD, GenerarReserva_62FD y GenerarPasaje_62FD**: el comentario que explica que los campos de estado guardan claves y no textos traducidos (uno por formulario). También el de la fecha de entrada al modo, que se conserva al cambiar de idioma (uno por formulario), y el de que se pasa la clave del nombre del modo a la barra de estado (siete en total entre los tres).
- **GenerarPasaje_62FD**: los dos comentarios que aclaran que label4 es el número de factura.
- **GestionUsuario_23DB**: "ex.Message viene de la BLL y no tiene clave".
- **MenuPrincipal_23DB**: "lblLoginMp y lblRol llevan datos de la sesión, los arma el formulario".
- **RegistrarPasajero_62FD**: el comentario del campo de clave del texto de estado.
- **IdiomaBLL_23DB**: los tres comentarios sobre el idioma base, el caché y su invalidación al cambiar de idioma.

Hay dos comentarios del proyecto que tienen la palabra "nuevo" y no son marcadores, y quedaron como estaban. Uno es la segunda línea del comentario de CambiarIdioma ("devuelva el idioma nuevo…"). El otro es "//admin nuevo", que ya estaba en btnAyuda_Click de MenuPrincipal_23DB. No toqué ningún otro comentario.

## Parte 5: limpiezas sueltas

En AuditoriaEventos_23DB.cs borré el campo actualizandoFecha_23DB. Con eso la advertencia CS0414 desapareció y el build quedó sin advertencias. Los dos manejadores ValueChanged vacíos de los DateTimePicker siguen ahí y siguen conectados en el Designer.

Confirmo lo de GestionUsuarios, sin sufijo. Son dos archivos en GUI_23DB, GestionUsuarios.cs y GestionUsuarios.Designer.cs, y no tiene .resx. No figura en GUI_23DB.csproj y ningún archivo de código lo referencia. Lo podés borrar sin que afecte el build.

## Lo que necesita tu revisión

1. **Los campos con guion bajo.** Las propiedades que empiezan con minúscula tienen el campo privado con guion bajo adelante, porque si no el nombre chocaba con el de la propiedad.
2. **Los dos DateTime sin valor vacío.** fechaHoraFin_62FD y FechaProximaRevision_62FD quedan en 01/01/0001 cuando no tienen valor, y eso además es incompatible con una columna datetime de SQL Server. Hay que decidir si pasan a DateTime?.
3. **Las multiplicidades 1 a 1 y 0..1.** Las de "Origina" y "Recibe" no se garantizan con las clases. En el DER necesitan una restricción UNIQUE sobre la clave foránea. El mínimo de una tarea por intervención tiene que validarlo la BLL.
4. **Propiedades de presentación que no agregué.** Te las propongo para cuando armes los formularios:
   - Matricula_62FD en Intervencion_62FD. Hoy solo se llega a la matrícula a través de la orden, y la grilla de intervenciones seguramente la va a mostrar.
   - Una lista de órdenes en Aeronave_62FD, si en algún caso de uso hace falta navegar desde la aeronave hacia sus órdenes.
   - Una referencia a la Certificacion_62FD desde Intervencion_62FD, para saber sin otra consulta si la intervención ya está certificada.
   - Una referencia a la Intervencion_62FD desde OrdenTrabajo_62FD.

   Cualquiera de estas cambia el diagrama de clases, por eso no las agregué.
5. **El estado previo del PN1.** Los archivos del PN1 ya estaban movidos a sus carpetas cuando empecé. Solo faltaba actualizar los .csproj.
6. **GestionUsuarios.** Está confirmado que nada lo usa, así que está listo para que lo borres.
