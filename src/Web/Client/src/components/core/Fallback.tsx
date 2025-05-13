import React from "react";

function Fallback(): React.JSX.Element {
  return (
    <div className="w-full h-full flex items-center justify-center">
      <div className="flex flex-row gap-2">
        <div className="w-2 h-2 rounded-full bg-blue-700 animate-bounce [animation-delay:.7s]" />
        <div className="w-2 h-2 rounded-full bg-blue-700 animate-bounce [animation-delay:.3s]" />
        <div className="w-2 h-2 rounded-full bg-blue-700 animate-bounce [animation-delay:.7s]" />
      </div>
    </div>
  );
}

export default Fallback;
