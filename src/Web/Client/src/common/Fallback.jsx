import React from "react";

function Fallback() {
  return (
    <div className="w-full h-full flex items-center justify-center bg-gray-200">
        <div className="flex flex-row gap-2">
          <div className="w-4 h-4 rounded-full bg-blue-700 animate-bounce [animation-delay:.7s]" />
          <div className="w-4 h-4 rounded-full bg-blue-700 animate-bounce [animation-delay:.3s]" />
          <div className="w-4 h-4 rounded-full bg-blue-700 animate-bounce [animation-delay:.7s]" />
        </div>
      </div>
  );
}

export default Fallback;
