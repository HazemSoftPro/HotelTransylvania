import React, { useEffect, useState } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import { Hotel, Sparkles, Moon, Sun } from 'lucide-react';

interface SplashScreenProps {
  onComplete: () => void;
  duration?: number;
}

export const SplashScreen: React.FC<SplashScreenProps> = ({ 
  onComplete, 
  duration = 3000 
}) => {
  const [isVisible, setIsVisible] = useState(true);
  const [currentPhase, setCurrentPhase] = useState(0);

  useEffect(() => {
    const phases = [
      { delay: 0, phase: 0 },
      { delay: 800, phase: 1 },
      { delay: 1600, phase: 2 },
      { delay: 2400, phase: 3 }
    ];

    phases.forEach(({ delay, phase }) => {
      setTimeout(() => setCurrentPhase(phase), delay);
    });

    const timer = setTimeout(() => {
      setIsVisible(false);
      setTimeout(onComplete, 500);
    }, duration);

    return () => clearTimeout(timer);
  }, [onComplete, duration]);

  const containerVariants = {
    hidden: { opacity: 0 },
    visible: { 
      opacity: 1,
      transition: { duration: 0.5 }
    },
    exit: { 
      opacity: 0,
      scale: 1.1,
      transition: { duration: 0.5 }
    }
  };

  const logoVariants = {
    hidden: { scale: 0, rotate: -180 },
    visible: { 
      scale: 1, 
      rotate: 0,
      transition: { 
        type: "spring",
        stiffness: 260,
        damping: 20,
        duration: 0.8
      }
    }
  };

  const textVariants = {
    hidden: { y: 50, opacity: 0 },
    visible: { 
      y: 0, 
      opacity: 1,
      transition: { 
        delay: 0.3,
        duration: 0.6,
        ease: "easeOut"
      }
    }
  };

  const sparkleVariants = {
    hidden: { scale: 0, opacity: 0 },
    visible: { 
      scale: [0, 1.2, 1], 
      opacity: [0, 1, 0.8],
      transition: { 
        delay: 0.6,
        duration: 0.8,
        repeat: Infinity,
        repeatType: "reverse" as const
      }
    }
  };

  const progressVariants = {
    hidden: { width: "0%" },
    visible: { 
      width: "100%",
      transition: { 
        delay: 1,
        duration: 1.8,
        ease: "easeInOut"
      }
    }
  };

  return (
    <AnimatePresence>
      {isVisible && (
        <motion.div
          className="fixed inset-0 z-50 flex items-center justify-center bg-gradient-to-br from-blue-900 via-purple-900 to-indigo-900"
          variants={containerVariants}
          initial="hidden"
          animate="visible"
          exit="exit"
        >
          {/* Background Pattern */}
          <div className="absolute inset-0 opacity-10">
            <div className="absolute inset-0 bg-[url('data:image/svg+xml,%3Csvg width="60" height="60" viewBox="0 0 60 60" xmlns="http://www.w3.org/2000/svg"%3E%3Cg fill="none" fill-rule="evenodd"%3E%3Cg fill="%23ffffff" fill-opacity="0.1"%3E%3Ccircle cx="30" cy="30" r="2"/%3E%3C/g%3E%3C/g%3E%3C/svg%3E')] bg-repeat"></div>
          </div>

          {/* Floating Icons */}
          <motion.div
            className="absolute top-20 left-20 text-white/20"
            animate={{ 
              y: [0, -20, 0],
              rotate: [0, 10, 0]
            }}
            transition={{ 
              duration: 3,
              repeat: Infinity,
              ease: "easeInOut"
            }}
          >
            <Moon size={32} />
          </motion.div>

          <motion.div
            className="absolute top-32 right-32 text-white/20"
            animate={{ 
              y: [0, 20, 0],
              rotate: [0, -10, 0]
            }}
            transition={{ 
              duration: 2.5,
              repeat: Infinity,
              ease: "easeInOut",
              delay: 0.5
            }}
          >
            <Sun size={28} />
          </motion.div>

          {/* Main Content */}
          <div className="relative text-center">
            {/* Logo Container */}
            <motion.div
              className="relative mb-8"
              variants={logoVariants}
              initial="hidden"
              animate="visible"
            >
              {/* Glow Effect */}
              <div className="absolute inset-0 bg-white/20 rounded-full blur-xl scale-150"></div>
              
              {/* Main Logo */}
              <div className="relative bg-white/10 backdrop-blur-sm rounded-full p-8 border border-white/20">
                <Hotel size={80} className="text-white mx-auto" />
                
                {/* Sparkles */}
                <motion.div
                  className="absolute -top-2 -right-2"
                  variants={sparkleVariants}
                  initial="hidden"
                  animate="visible"
                >
                  <Sparkles size={24} className="text-yellow-300" />
                </motion.div>
                
                <motion.div
                  className="absolute -bottom-2 -left-2"
                  variants={sparkleVariants}
                  initial="hidden"
                  animate="visible"
                  style={{ animationDelay: '0.3s' }}
                >
                  <Sparkles size={20} className="text-blue-300" />
                </motion.div>
              </div>
            </motion.div>

            {/* Title */}
            <motion.div
              variants={textVariants}
              initial="hidden"
              animate="visible"
              className="mb-4"
            >
              <h1 className="text-5xl font-bold text-white mb-2 tracking-wide">
                Hotel Transylvania
              </h1>
              <p className="text-xl text-white/80 font-light">
                إدارة الفنادق الذكية
              </p>
            </motion.div>

            {/* Loading Text */}
            <motion.div
              className="mb-8"
              initial={{ opacity: 0 }}
              animate={{ opacity: 1 }}
              transition={{ delay: 1.2 }}
            >
              <p className="text-white/60 text-sm">
                {currentPhase === 0 && "جاري التحميل..."}
                {currentPhase === 1 && "تحضير الواجهة..."}
                {currentPhase === 2 && "تحميل البيانات..."}
                {currentPhase === 3 && "تقريباً انتهينا..."}
              </p>
            </motion.div>

            {/* Progress Bar */}
            <motion.div
              className="w-64 h-1 bg-white/20 rounded-full mx-auto overflow-hidden"
              initial={{ opacity: 0, scale: 0.8 }}
              animate={{ opacity: 1, scale: 1 }}
              transition={{ delay: 1 }}
            >
              <motion.div
                className="h-full bg-gradient-to-r from-blue-400 to-purple-400 rounded-full"
                variants={progressVariants}
                initial="hidden"
                animate="visible"
              />
            </motion.div>

            {/* Version */}
            <motion.div
              className="absolute bottom-8 left-1/2 transform -translate-x-1/2"
              initial={{ opacity: 0 }}
              animate={{ opacity: 1 }}
              transition={{ delay: 2 }}
            >
              <p className="text-white/40 text-xs">
                الإصدار 1.0.0
              </p>
            </motion.div>
          </div>

          {/* Animated Particles */}
          {[...Array(6)].map((_, i) => (
            <motion.div
              key={i}
              className="absolute w-2 h-2 bg-white/30 rounded-full"
              style={{
                left: `${20 + i * 15}%`,
                top: `${30 + (i % 2) * 40}%`,
              }}
              animate={{
                y: [0, -100, 0],
                opacity: [0, 1, 0],
                scale: [0, 1, 0],
              }}
              transition={{
                duration: 3,
                repeat: Infinity,
                delay: i * 0.5,
                ease: "easeInOut",
              }}
            />
          ))}
        </motion.div>
      )}
    </AnimatePresence>
  );
};

export default SplashScreen;
