Buen día estimados

Aqui ire modificando las rutas públicas tanto de la api como de la web que se harán a traves de AWS. Esto ya que al apagar las instancias pueden cambiar las Ips de los servicios, por lo que si quieren validarlos deben de ingresar a los siguientes links

Tener en cuenta que por temas de costos ambos servicios se estarán prendiendo en el horario de 9 de la mañana a 4 de la tarde a partir del 12/01/2026 y terminando el 16/01/2026

***Procederé a eliminar las instancias de la api y de la web*** 


API: http://18.222.67.187:5000/swagger/index.html

WEB: http://18.226.4.187:5001/

**Puntos a tomar en cuenta al validar:
  -Se agrego la autenticación por medio de JWE por lo que para ingresar van a tener que colocar un correo y contraseña validos que puede ser cualquiera de los siguientes:
  CORREO: admin@neonet.com	PASSWORD: 123
  CORREO:	vendedor@neonet.com	 PASSWORD: 123
Cada uno de ellos con diferentes menus debido a la lógica que se implemento de roles.
Adicional se implementó lo que es el recaptcha para hacer mas robusto el tema del login. (Cabe aclarar que la versión subida en la web no tiene ya que como la verificación del recaptcha va a nivel de dominio y google no permite que se despliegue a traves del protocolo http a no ser que sea en localhost)

Si quieren probar cualquier endpoint siempre tienen que generar un Token utilizando el endpoint de Autenticación. En el campo de tokenCaptcha pueden mandarlo como vacio ya que de momento y para agilizar las pruebas se agrego una validación a nivel del appsettins que valida o no si el token que le mandan ahi es válido.









  

