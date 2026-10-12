export default function Loading({ text = "Učitavanje..." }) {
  return (
    <div className="loading">
      <span className="spinner" />
      <span>{text}</span>
    </div>
  );
}
