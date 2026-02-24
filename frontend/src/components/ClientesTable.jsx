export default function ClientesTable({ clientes, onEdit, onDelete, loading }) {
  if (loading) return <p>Cargando clientes...</p>

  if (!clientes.length) return <p>No hay clientes registrados.</p>

  return (
    <table>
      <thead>
        <tr>
          <th>ID</th>
          <th>Nombre</th>
          <th>Edad</th>
          <th>Fecha Nacimiento</th>
          <th>Salario</th>
          <th>Acciones</th>
        </tr>
      </thead>
      <tbody>
        {clientes.map((cliente) => (
          <tr key={cliente.clienteId}>
            <td>{cliente.clienteId}</td>
            <td>{cliente.nombre}</td>
            <td>{cliente.edad}</td>
            <td>{new Date(cliente.fechaNacimiento).toLocaleDateString()}</td>
            <td>{Number(cliente.salario).toLocaleString('es-ES', { style: 'currency', currency: 'EUR' })}</td>
            <td>
              <button type="button" onClick={() => onEdit(cliente.clienteId)}>Editar</button>
              <button type="button" className="danger" onClick={() => onDelete(cliente.clienteId)}>Eliminar</button>
            </td>
          </tr>
        ))}
      </tbody>
    </table>
  )
}
