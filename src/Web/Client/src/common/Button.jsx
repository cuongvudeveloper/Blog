import PropTypes from "prop-types";
import React from "react";

function Button({ children, className, ...props }) {
  return (
    <button
      className={`px-3 py-1 w-full rounded-md cursor-pointer bg-black text-white hover:bg-gray-800 ${className}`}
      {...props}
    >
      {children}
    </button>
  );
}

Button.propTypes = {
  children: PropTypes.node,
  className: PropTypes.string,
  onClick: PropTypes.func,
};

export default Button;
